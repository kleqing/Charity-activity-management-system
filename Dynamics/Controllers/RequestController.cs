using Microsoft.AspNetCore.Mvc;
using Dynamics.DataAccess.Repository;
using Dynamics.Models.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Dynamics.Utility;

namespace Dynamics.Controllers
{
	public class RequestController : Controller
	{
		private readonly IRequestRepository _requestRepo;
		private readonly IUserRepository _userRepo;
		private readonly UserManager<IdentityUser> _userManager;

		public RequestController(IRequestRepository requestRepository, IUserRepository userRepo, UserManager<IdentityUser> userManager)
		{
			_requestRepo = requestRepository;
			_userRepo = userRepo;
			_userManager = userManager;
		}
		public async Task<IActionResult> Index()
		{
			return View(await _requestRepo.GetAllAsync());
		}
		public async Task<IActionResult> MyRequest()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return Unauthorized();
			}
			var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
			Guid userId = Guid.Empty;
			var userJson = HttpContext.Session.GetString("user");
			if (!string.IsNullOrEmpty(userJson))
			{
				var userJsonC = JsonConvert.DeserializeObject<User>(userJson);
				userId = userJsonC.UserID;
			}
			var request = await _requestRepo.GetAllByRoleAsync(role, userId);
			return View(request);
		}
		public async Task<IActionResult> Detail(Guid? id)
		{
			Guid userId = Guid.NewGuid();
			
			var request = await _requestRepo.GetAsync(r => r.RequestID.Equals(id));
			if (request == null) { return NotFound(); }
			return View(request);
		}
		public IActionResult Create()
		{
			return View();
		}
		//TODO: handle image upload
		[HttpPost]
		public async Task<IActionResult> Create(Request obj)
		{
			var date = DateOnly.FromDateTime(DateTime.Now);
			obj.CreationDate = date;
			var userJson = HttpContext.Session.GetString("user");
			if (!string.IsNullOrEmpty(userJson))
			{
				var user = JsonConvert.DeserializeObject<User>(userJson);
				obj.UserID = user.UserID;
				await _requestRepo.AddAsync(obj);
			}
			return RedirectToAction("Index", "Request");
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
			Request request = await _requestRepo.GetByRoleAsync(r => r.RequestID.Equals(id), role, userId);
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
		public async Task<IActionResult> Edit(Request obj, List<IFormFile> images)
		{
			/*if (!ModelState.IsValid)
			{
				return View(obj);
			}*/
			// Get the currently logged-in user (role and id)
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
			// Get existing request
			Request existingRequest = await _requestRepo.GetByRoleAsync(r => r.RequestID.Equals(obj.RequestID), role, userId);
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
					string imagePath = Util.UploadMultiImage(images, @"images\Requests", userId);
					// append new images if there are existing images
					if (!string.IsNullOrEmpty(imagePath))
					{
						existingRequest.Attachment = string.IsNullOrEmpty(existingRequest.Attachment) ? imagePath 
							: existingRequest.Attachment + "," + imagePath;
					}
				}
			}
			/*TODO: make a separate method to handle update request status by admin
			else if (role == "Admin")
			{
				// If the user is "admin", allow them to update only the Status field
				existingRequest.Status = obj.Status;
			}*/
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
			_requestRepo.DeleteAsync(request);
			return RedirectToAction("MyRequest", "Request");
		}
	}
}
