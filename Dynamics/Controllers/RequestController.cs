using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Dynamics.DataAccess.Repository;
using Dynamics.Models.Models;
using Dynamics.Models.Models.ViewModel;
using Dynamics.Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Dynamics.Utility;
using Microsoft.EntityFrameworkCore;
using Controller = Microsoft.AspNetCore.Mvc.Controller;
using ILogger = Serilog.ILogger;

namespace Dynamics.Controllers
{
	public class RequestController : Controller
	{
		private readonly IRequestRepository _requestRepo;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly ILogger<RequestController> _logger;
		private readonly IRequestService _requestService;

		public RequestController(IRequestRepository requestRepository, UserManager<IdentityUser> userManager, ILogger<RequestController> logger, IRequestService requestService)
		{
			_requestRepo = requestRepository;
			_userManager = userManager;
			_logger = logger;
			_requestService = requestService;
		}
		
		// View all requests
		public async Task<IActionResult> Index(string searchQuery, string filterQuery, int pageNumber = 1, int pageSize = 12)
		{
			User currentUser = HttpContext.Session.GetCurrentUser();
			var paginatedRequests = await _requestRepo.GetAllAsync(pageNumber, pageSize);
			int total = _requestRepo.CountRequests(r => r.UserID == currentUser.UserID);
			// search
			if (!string.IsNullOrEmpty(searchQuery))
			{
				paginatedRequests = await _requestRepo.SearchIndexFilterAsync(searchQuery, filterQuery).AsQueryable().ToListAsync();
			}
			// Take the celling instead of nearest number
			var totalPages = (int)Math.Ceiling((double)total / pageSize);
			
			ViewBag.currentPage = pageNumber;
			ViewBag.totalPages = totalPages;
			return View(paginatedRequests);
		}
		
		[Authorize]
		public async Task<IActionResult> MyRequest(string searchQuery, string filterQuery, int pageNumber = 1, int pageSize = 12)
		{
			User currentUser = HttpContext.Session.GetCurrentUser();
			// Get requests of user (Paginated, should only contain 9 result and a count for all pages)
			var paginatedRequests = await _requestRepo.GetAllAsync(pageNumber, pageSize);
			// Count of total requests
			int total = _requestRepo.CountRequests(r => r.UserID == currentUser.UserID);
			
			// search (optional)
			if (!string.IsNullOrEmpty(searchQuery))
			{
				var temp = _requestRepo.SearchIdFilter(searchQuery, filterQuery, currentUser.UserID);
				paginatedRequests = await temp.AsQueryable().ToListAsync(); // Execute query here
			}
			//pagination
			var totalPages = (int)Math.Ceiling((double)total / pageSize);
			
			ViewBag.currentPage = pageNumber;
			ViewBag.totalPages = totalPages;
			
			// Convert to view models
			var requestOverviewDto = _requestService.MapToListRequestOverviewDto(paginatedRequests);
			return View(requestOverviewDto);
		}
		public async Task<IActionResult> Detail(Guid id)
		{
			var request = await _requestRepo.GetAsync(r => r.RequestID.Equals(id));
			if (request == null) { return NotFound(); }
			return View(request);
		}
		public IActionResult Create()
		{
			var userJson = HttpContext.Session.GetString("user");
			var user  = JsonConvert.DeserializeObject<User>(userJson);

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
		[Microsoft.AspNetCore.Mvc.HttpPost]
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
				string imagePath = Util.UploadMultiImage(images, $@"images\Requests\" + obj.RequestID.ToString(), userId);
				obj.Attachment = imagePath;
			}
			else
			{
				obj.Attachment = "/images/Requests/Placeholder/xddFaker.png";
			}
			_logger.LogInformation("Request created");
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
			_logger.LogInformation("Get logged-in user");
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return Unauthorized();
			}
			var userId = Guid.Parse(user.Id);
			var request = await _requestRepo.GetAsync(r => r.RequestID.Equals(id));
			if (request == null)
			{
				return NotFound();
			}
			if (request.UserID != userId)
			{
				return Forbid(); // If the user is not the owner of the request
			}

            return View(request);
		}
		[Microsoft.AspNetCore.Mvc.HttpPost]
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
			var userId = Guid.Parse(user.Id);
			// Get existing request
			_logger.LogInformation("Get existing request");
			var existingRequest = await _requestRepo.GetAsync(r => r.RequestID.Equals(obj.RequestID));
			if (existingRequest == null)
			{
				return NotFound();
			}
			existingRequest.RequestTitle = obj.RequestTitle;
			existingRequest.Content = obj.Content;
			existingRequest.RequestEmail = obj.RequestEmail;
			existingRequest.RequestPhoneNumber = obj.RequestPhoneNumber;
			existingRequest.Location = obj.Location;
			existingRequest.isEmergency = obj.isEmergency;
			if (images != null && images.Count > 0)
			{
				string imagePath = Util.UploadMultiImage(images, $@"images\Requests\" + existingRequest.RequestID.ToString(), userId);
				// append new images if there are existing images
				if (!string.IsNullOrEmpty(imagePath))
				{
					existingRequest.Attachment = string.IsNullOrEmpty(existingRequest.Attachment) ? imagePath 
						: existingRequest.Attachment + "," + imagePath;
				}
			}
			_logger.LogInformation("Edit request");
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
		[Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.ActionName("Delete")]
		public async Task<IActionResult> DeletePost(Guid? id)
		{
			_logger.LogInformation("Get deleted request");
			Request request = await _requestRepo.GetAsync(r => r.RequestID.Equals(id));
			if (request == null) { return NotFound(); };
			_logger.LogInformation("Delete request");
			await _requestRepo.DeleteAsync(request);
			return RedirectToAction("MyRequest", "Request");
		}
	}
}
