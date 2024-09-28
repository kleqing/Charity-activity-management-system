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
        private readonly SignInManager<IdentityUser> _signInManager;

        public UserController(IUserRepository userRepo, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userRepository = userRepo;
            _userManager = userManager;
            _signInManager = signInManager;
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
                // Update the session as well
                HttpContext.Session.SetString("user", JsonConvert.SerializeObject(user));
                TempData[MyConstants.Success] = "User updated!";
            }
            catch (Exception ex)
            {
                TempData[MyConstants.Failed] = "Update failed...";
            }

            return View(user);
        }

        public async Task<IActionResult> Account(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user.PasswordHash == null)
            {
                TempData["Google"] = "Your account is bounded with google account.";
            }
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePassword)
        {
            // TODO: Check if user has password or not
            
            if (!ModelState.IsValid)
            {
                TempData[MyConstants.Failed] = "Change password failed...";
                return RedirectToAction("Account", "User");
            }

            if (changePassword.NewPassword != changePassword.ConfirmPassword)
            {
                TempData[MyConstants.Failed] = "New password and confirmation password do not match...";
                return RedirectToAction("Account", "User");
            }
            var currentUser = await _userManager.FindByIdAsync(changePassword.UserId.ToString());
            if (currentUser.PasswordHash == null)
            {
                TempData["Google"] = "Your account is bound with google account.";
                return RedirectToAction("Account", "User");
            }
            var changePasswordResult = await _userManager.ChangePasswordAsync(currentUser, changePassword.OldPassword, changePassword.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    TempData[MyConstants.Failed] = error.Description;
                }
                return RedirectToAction("Account", "User");
            }
            // Add a message and refresh the page
            TempData[MyConstants.Success] = "Password changed!";
            await _signInManager.RefreshSignInAsync(currentUser);
            // return RedirectToAction("Logout", "Auth");
            return RedirectToAction("Account", "User");
        }
    }
}