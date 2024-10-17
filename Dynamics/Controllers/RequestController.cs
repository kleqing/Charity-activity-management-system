using Dynamics.DataAccess.Repository;
using Dynamics.Models.Models;
using Dynamics.Models.Models.ViewModel;
using Dynamics.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dynamics.Controllers
{
    public class RequestController : Controller
    {
        private readonly IRequestRepository _requestRepo;
        private readonly IUserRepository _userRepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly CloudinaryUploader _cloudinaryUploader;

        public RequestController(IRequestRepository requestRepository, IUserRepository userRepo,
            UserManager<IdentityUser> userManager, CloudinaryUploader cloudinaryUploader)
        {
            _requestRepo = requestRepository;
            _userRepo = userRepo;
            _userManager = userManager;
            _cloudinaryUploader = cloudinaryUploader;
        }

        //TODO: implement filter for each field (search)
        public async Task<IActionResult> Index(string searchQuery, int pageNumber = 1, int pageSize = 12)
        {
            var requests = await _requestRepo.GetAllPaginatedAsync(pageNumber, pageSize);
            if (!string.IsNullOrEmpty(searchQuery))
            {
                requests = await _requestRepo.SearchIndexFilterAsync(searchQuery);
            }

            var totalRequest = 0;
            foreach (var request in requests)
            {
                totalRequest++;
            }

            var totalPages = (int)Math.Ceiling((double)totalRequest / pageSize);

            ViewBag.currentPage = pageNumber;
            ViewBag.totalPages = totalPages;
            return View(requests);
        }

        public async Task<IActionResult> MyRequest(string searchQuery, int pageNumber = 1, int pageSize = 12)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            if (role == null) throw new Exception("WARNING: User has no role");
            Guid userId = Guid.Empty;
            var userJson = HttpContext.Session.GetString("user");
            if (!string.IsNullOrEmpty(userJson))
            {
                var userJsonC = JsonConvert.DeserializeObject<User>(userJson);
                userId = userJsonC.UserID;
            }

            var requests = await _requestRepo.GetAllByIdPaginatedAsync(role, userId, pageNumber, pageSize);

            // search
            if (!string.IsNullOrEmpty(searchQuery))
            {
                requests = await _requestRepo.SearchIdFilterAsync(searchQuery, userId);
            }

            //pagination
            var totalRequest = 1 + requests.Count;
            var totalPages = (int)Math.Ceiling((double)totalRequest / pageSize);

            ViewBag.currentPage = pageNumber;
            ViewBag.totalPages = totalPages;
            return View(requests);
        }

        public async Task<IActionResult> Detail(Guid? id)
        {
            var request = await _requestRepo.GetAsync(r => r.RequestID.Equals(id));
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        public IActionResult Create()
        {
            var userJson = HttpContext.Session.GetString("user");
            var user = JsonConvert.DeserializeObject<User>(userJson);

            var viewModel = new RequestCreateVM
            {
                UserEmail = user.UserEmail,
                UserPhoneNumber = user.UserPhoneNumber,
                UserAddress = user.UserAddress,

                RequestTitle = string.Empty,
                Content = string.Empty,
                CreationDate = null,
                RequestEmail = string.Empty,
                RequestPhoneNumber = string.Empty,
                Location = string.Empty,
                isEmergency = 0
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Request obj, List<IFormFile> images,
            string? cityNameInput, string? districtNameInput, string? wardNameInput)
        {
            obj.RequestID = Guid.NewGuid();
            /*var date = DateOnly.FromDateTime(DateTime.Now);
            obj.CreationDate = date;*/
            obj.Location += ", " + wardNameInput + ", " + districtNameInput + ", " + cityNameInput;
            var userId = Guid.Empty;
            var userJson = HttpContext.Session.GetString("user");
            if (!string.IsNullOrEmpty(userJson))
            {
                var user = JsonConvert.DeserializeObject<User>(userJson);
                userId = user.UserID;
            }

            obj.UserID = userId;
            if (images != null && images.Count > 0)
            {
                // string imagePath = Util.UploadMultiImage(images, $@"images\Requests\" + obj.RequestID.ToString(), userId);
                obj.Attachment = await _cloudinaryUploader.UploadMultiImagesAsync(images);
            }
            else
            {
                obj.Attachment = "/images/Requests/Placeholder/xddFaker.png";
            }

            await _requestRepo.AddAsync(obj);
            return RedirectToAction("MyRequest", "Request");
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "User";
            Guid userId = Guid.Empty;
            var userJson = HttpContext.Session.GetString("user");
            if (!string.IsNullOrEmpty(userJson))
            {
                var userJsonC = JsonConvert.DeserializeObject<User>(userJson);
                userId = userJsonC.UserID;
            }

            var request = await _requestRepo.GetByIdAsync(r => r.RequestID.Equals(id), role, userId);
            if (request == null)
            {
                return NotFound();
            }

            if (role == "User" && request.UserID != userId)
            {
                return Forbid(); // If the user is not the owner of the request
            }

            return View(request);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Request obj, List<IFormFile> images,
            string? cityNameInput, string? districtNameInput, string? wardNameInput)
        {
            /*if (!ModelState.IsValid)
			{
				return View(obj);
			}*/
            // Get the currently logged-in user (role and id)
            obj.Location += ", " + wardNameInput + ", " + districtNameInput + ", " + cityNameInput;
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "User";
            var userId = Guid.Empty;
            var userJson = HttpContext.Session.GetString("user");
            if (!string.IsNullOrEmpty(userJson))
            {
                var userJsonC = JsonConvert.DeserializeObject<User>(userJson);
                userId = userJsonC.UserID;
            }
            // Get existing request
            var existingRequest = await _requestRepo.GetByIdAsync(r => r.RequestID.Equals(obj.RequestID), role, userId);
            if (existingRequest == null)
            {
                return NotFound();
            }
            if (role == "User")
            {
                // If the user is "user", allow them to update only certain fields
                existingRequest.Content = obj.Content;
                existingRequest.Location = obj.Location;
                existingRequest.isEmergency = obj.isEmergency;
                if (images != null && images.Count > 0)
                {
                    string imagePath = await _cloudinaryUploader.UploadMultiImagesAsync(images);
                    // append new images if there are existing images
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        existingRequest.Attachment = string.IsNullOrEmpty(existingRequest.Attachment) ? imagePath
                            : existingRequest.Attachment + "," + imagePath;
                    }
                }
            }
            await _requestRepo.UpdateAsync(existingRequest);
            return RedirectToAction("MyRequest", "Request");
        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Request request = await _requestRepo.GetAsync(r => r.RequestID.Equals(id));
            if (request == null) { return NotFound(); }
            return View(request);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePost(Guid? id)
        {
            Request request = await _requestRepo.GetAsync(r => r.RequestID.Equals(id));
            if (request == null) { return NotFound(); };
            await _requestRepo.DeleteAsync(request);
            return RedirectToAction("MyRequest", "Request");
        }

        public IActionResult AcceptRequest(Guid requestId)
        {
            return RedirectToAction("CreateProject", "Project", new { requestId = requestId });
        }
    }
}