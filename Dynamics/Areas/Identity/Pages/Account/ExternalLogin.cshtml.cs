#nullable disable

using Dynamics.DataAccess.Repository;
using Dynamics.Models.Models;
using Dynamics.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Dynamics.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;
        private readonly IUserRepository _userRepo;

        public ExternalLoginModel(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender,
            IUserRepository userRepo)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _logger = logger;
            _emailSender = emailSender;
            _userRepo = userRepo;
        }

        [BindProperty] public InputModel Input { get; set; }

        public string ProviderDisplayName { get; set; }

        public string ReturnUrl { get; set; }

        [TempData] public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required][EmailAddress] public string Email { get; set; }
        }

        // Prevent unauthorized access to the page
        public IActionResult OnGet() => RedirectToPage("./Login");


        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        // Login
        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Checking if the user already have an account with the same email, we will use that account to log in instead
            var userEmail = info.Principal.FindFirstValue(ClaimTypes.Email);
            var existingUser = await _userManager.FindByEmailAsync(userEmail ?? "No email");
            if (existingUser != null)
            {
                // Sign in using that user instead of Google
                var businessUser = await _userRepo.GetAsync(u => u.UserEmail == userEmail);
                HttpContext.Session.SetString("user", JsonConvert.SerializeObject(businessUser));
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.",
                    info.Principal.Identity.Name, info.LoginProvider);
                await _signInManager.SignInAsync(existingUser, isPersistent: false, authenticationMethod: null);
                return RedirectToAction("Homepage", "Home");
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                // Set the session
                var businessUser = await _userRepo.GetAsync(u => u.UserEmail == userEmail);
                HttpContext.Session.SetString("user", JsonConvert.SerializeObject(businessUser));
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.",
                    info.Principal.Identity.Name,
                    info.LoginProvider);

                return RedirectToAction("Homepage", "Home");
            }

            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                // Do we need this though ?
                ReturnUrl = returnUrl;
                ProviderDisplayName = info.ProviderDisplayName;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                }
            }

            return Page();
        }

        // Register
        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid)
            {
                var user = CreateUser();
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);

                    var existed = await _userManager.FindByEmailAsync(user.Email);
                    if (result.Succeeded && existed != null)
                    {
                        // Add user to the database after creating the user with external login
                        await _userRepo.Add(new User
                        {
                            UserID = new Guid(user.Id),
                            UserFullName =
                                info.Principal.FindFirstValue(ClaimTypes.Name), // Get user's name from Google
                            UserEmail = info.Principal.FindFirstValue(ClaimTypes.Email), // Get user's email from Google
                            UserAvatar = info.Principal.FindFirstValue("picture")
                        });

                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                        existed.EmailConfirmed = true;
                        await _userManager.UpdateAsync(existed);

                        // We don't need to confirm the email if the user use Google auth

                        await _emailSender.SendEmailAsync(Input.Email, "Register Confirmation", $"You have register successfully to Dynamics");

                        // Set the session for the app:
                        var businessUser = await _userRepo.GetAsync(u => u.UserEmail == user.Email);
                        HttpContext.Session.SetString("user", JsonConvert.SerializeObject(businessUser));
                        // Provide user with default role as well
                        result = _userManager.AddToRoleAsync(user, RoleConstants.User).GetAwaiter().GetResult();
                        if (result.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                            // TODO: Redirect to homepage instead
                            return RedirectToAction("HomePage", "Home");
                        }
                        // else
                        // {
                        //     return RedirectToAction("Index", "Home");
                        // }
                    }
                    // else
                    // {
                    //     // TODO: Bind the google account with current account
                    //     ModelState.AddModelError("Email", "Email already exists in the system.");
                    // }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ProviderDisplayName = info.ProviderDisplayName;
            ReturnUrl = returnUrl;
            return Page();
        }


        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                                                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                                                    $"override the external login page in /Areas/Identity/Pages/Account/ExternalLogin.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }

            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
