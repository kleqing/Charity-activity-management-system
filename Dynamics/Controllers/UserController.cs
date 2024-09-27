using Dynamics.DataAccess.Repository;
using Dynamics.Models.Models;
using Dynamics.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dynamics.Controllers
{
    [Authorize(Roles = RoleConstants.User)]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(IUserRepository userRepo, UserManager<IdentityUser> userManager)
        {
            _userRepository = userRepo;
            _userManager = userManager;
        }

        // User/username
        public async Task<IActionResult> Index(string username)
        {
            var currentUser = await _userRepository.GetAsync(u => u.UserFullName.Equals(username));
            if (currentUser == null) return RedirectToAction("Index", "Home");
            return View(currentUser);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            var user = await _userRepository.GetAsync(u => u.UserID.Equals(id));

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Client/Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User user, IFormFile image)
        {
            try
            {
                if (image != null)
                {
                    user.UserAvatar = Util.UploadImage(image, @"images\User", user.UserID.ToString());
                }
                await _userRepository.Update(user);
                //Update the session as well
                HttpContext.Session.SetString("user", JsonConvert.SerializeObject(user));
                TempData[MyConstants.Success] = "User updated!";
            }
            catch (Exception ex)
            {
                TempData[MyConstants.Failed] = "Update failed...";
            }
    
            return View(user);
        }
    }
}