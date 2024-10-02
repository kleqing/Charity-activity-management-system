using Dynamics.DataAccess.Repository;
using Dynamics.Models.Models;
using Dynamics.Models.Models.ViewModel;
using Dynamics.Services;
using Dynamics.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace Dynamics.Controllers
{
    [Authorize(Roles = RoleConstants.User)]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ITransactionViewService _transactionViewService;
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IOrganizationMemberRepository _organizationMemberRepository;

        public UserController(IUserRepository userRepo, UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, ITransactionViewService transactionViewService,
            IProjectMemberRepository projectMemberRepository, IOrganizationMemberRepository organizationMemberRepository)
        {
            _userRepository = userRepo;
            _userManager = userManager;
            _signInManager = signInManager;
            _transactionViewService = transactionViewService;
            _projectMemberRepository = projectMemberRepository;
            _organizationMemberRepository = organizationMemberRepository;
        }

        // User/username
        public async Task<IActionResult> Index(string username)
        {
            var currentUser = await _userRepository.GetAsync(u => u.UserFullName.Equals(username));
            if (currentUser == null) return RedirectToAction("Index", "Home");
            return View(currentUser);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var userString = HttpContext.Session.GetString("user");
            User currentUser = null;
            if (userString != null)
            {
                currentUser = JsonConvert.DeserializeObject<User>(userString);
            }
            else
            {
                return RedirectToAction("Logout", "Auth");
            }
            var user = await _userRepository.GetAsync(u => u.UserID.Equals(currentUser.UserID));
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Client/Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User user, IFormFile? image)
        {
            try
            {
                var existingUser = await _userRepository.GetAsync(u => u.UserFullName.Equals(user.UserFullName));
                existingUser = await _userRepository.GetAsync(u => u.UserEmail.Equals(user.UserEmail));
                if (existingUser != null && (existingUser.UserFullName != user.UserFullName ||
                                             existingUser.UserEmail != user.UserEmail))
                {
                    ModelState.AddModelError("", "Username or email is already taken.");
                    return View(user);
                }
                if (user.UserFullName == null || user.UserEmail == null)
                {
                    TempData[MyConstants.Failed] = "Update failed...";
                    return View(user);
                }
                if (image != null)
                {
                    user.UserAvatar = Util.UploadImage(image, @"images\User", user.UserID.ToString());
                }
                await _userRepository.UpdateAsync(user);
                // Update the session as well
                HttpContext.Session.SetString("user", JsonConvert.SerializeObject(user));
                TempData[MyConstants.Success] = "User updated!";
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Something went wrong, please try again later.");
                return View(user);
            }
            return View(user);
        }

        public async Task<IActionResult> Account()
        {
            var userString = HttpContext.Session.GetString("user");
            User currentUser = null;
            if (userString != null)
            {
                currentUser = JsonConvert.DeserializeObject<User>(userString);
            }
            else
            {
                return RedirectToAction("Logout", "Auth");
            }

            var user = await _userManager.FindByIdAsync(currentUser.UserID.ToString());
            if (user.PasswordHash == null)
            {
                TempData["Google"] = "Your account is bounded with google account.";
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePassword)
        {
            if (!ModelState.IsValid)
            {
                TempData[MyConstants.Failed] = "Change password failed...";
                return RedirectToAction("Account", "User");
            }
            if (changePassword.NewPassword != changePassword.ConfirmPassword)
            {
                TempData[MyConstants.Failed] = "The password and confirmation password do not match.";
                return RedirectToAction("Account", "User");
            }
            var currentUser = await _userManager.FindByIdAsync(changePassword.UserId.ToString());
            if (currentUser.PasswordHash == null)
            {
                TempData["Google"] = "Your account is bound with google account.";
                return RedirectToAction("Account", "User");
            }
            var changePasswordResult = await _userManager.ChangePasswordAsync(currentUser, changePassword.OldPassword,
                changePassword.NewPassword);
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

        /**
         * Display all accepted user donations
         */
        public async Task<IActionResult> History()
        {
            // Get current userID
            var userString = HttpContext.Session.GetString("user");
            User currentUser = null;
            if (userString != null) currentUser = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"));
            else return RedirectToAction("Logout", "Auth");
            var userToOrgTransaction = await _transactionViewService
                .GetUserToOrganizationTransactionDTOsAsync(ut => ut.UserID.Equals(currentUser.UserID) && ut.Status == 1);
            var userToPrjTransaction = await _transactionViewService
                .GetUserToProjectTransactionDTOsAsync(ut => ut.UserID.Equals(currentUser.UserID) && ut.Status == 1);
            // Merge into a list and display
            var total = new List<UserTransactionDto>();
            total.AddRange(userToOrgTransaction);
            total.AddRange(userToPrjTransaction);
            var final = new UserHistoryViewModel
            {
                userTransactions = total.OrderByDescending(ut => ut.Time).ToList() // default
            };
            return View(final);
        }

        public async Task<IActionResult> RequestsStatus()
        {
            // Get current userID
            var userString = HttpContext.Session.GetString("user");
            User currentUser = null;
            if (userString != null) currentUser = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"));
            else return RedirectToAction("Logout", "Auth");
            // Join requests
            var userRequestProjects = await _projectMemberRepository.GetAllAsync(pm => pm.UserID.Equals(currentUser.UserID));
            var userRequestOrganizations = await _organizationMemberRepository.GetAllAsync(pm => pm.UserID.Equals(currentUser.UserID));
            // Donation requests only get the pending ones (Money is automatically accepted so it should not be here.)
            var userToOrgTransaction = await _transactionViewService
                .GetUserToOrganizationTransactionDTOsAsync(ut => ut.UserID.Equals(currentUser.UserID) && ut.Status == 0);
            var userToPrjTransaction = await _transactionViewService
                .GetUserToProjectTransactionDTOsAsync(ut => ut.UserID.Equals(currentUser.UserID) && ut.Status == 0);
            userToPrjTransaction.AddRange(userToOrgTransaction);
            
            return View(new UserRequestsStatusViewModel
            {
                OrganizationJoinRequests = userRequestOrganizations,
                ProjectJoinRequests = userRequestProjects,
                ResourcesDonationRequests = userToPrjTransaction,
            });
        }
    }
}