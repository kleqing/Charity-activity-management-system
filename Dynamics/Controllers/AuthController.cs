using Dynamics.DataAccess.Repository;
using Dynamics.Models.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;

namespace Dynamics.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserRepository _userRepository;
        
        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IUserRepository userRepo)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userRepository = userRepo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> ChangePassword()
        {
            //get current user
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            //check having pwd
            var hasPassword = await _userManager.HasPasswordAsync(currentUser);
            if (!hasPassword)
            {
                //nananaa
                return RedirectToAction("Index", "Home");
            }
            //assign current user to userDto
            ViewData["Title"] = "Change password";
            var userDto = new ChangePasswordDto() { UserId = new Guid(currentUser.Id) };
            return View(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePassword)
        {
            // TODO: CHeck if user has password or not
            await _signInManager.SignOutAsync();

            if (!ModelState.IsValid)
            {
                return RedirectToAction("ChangePassword", "Auth");
            }
            var currentUser = await _userManager.FindByIdAsync(changePassword.UserId.ToString());

            var changePasswordResult = await _userManager.ChangePasswordAsync(currentUser, changePassword.OldPassword, changePassword.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return RedirectToAction("ChangePassword", "Auth");
            }
            //update success, go to login again
            await _signInManager.RefreshSignInAsync(currentUser);

            // return RedirectToAction("Logout", "Auth");
            return RedirectToPage("/Account/login", new {area = "Identity"});
        }

    }
}
