// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using Dynamics.DataAccess.Repository;
using Dynamics.Models.AuthModels;
using Dynamics.Models.Models;
using Dynamics.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Newtonsoft.Json;

namespace Dynamics.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserRepository _userRepo;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            IUserRepository repository
        )
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _userRepo = repository;
        }

        // Declares that incoming http request will be bind to this input
        // This only appear in Razor page because they have no controller
        [BindProperty] public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required] public string Name { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                // Changed this one so that it will use our custom identity user
                // Click on it to see the implementation
                // Also because we are using custom user,
                // we need to update the implementation for email sender as well (Implemented in Utility)
                var user = CreateUser();
                // If role not exist, create all of our possible role
                // also, getAwaiter is the same as writing await keyword
                if (!_roleManager.RoleExistsAsync(RoleConstants.User).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(RoleConstants.User)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(RoleConstants.Admin)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(RoleConstants.HeadOfOrganization)).GetAwaiter()
                        .GetResult();
                    _roleManager.CreateAsync(new IdentityRole(RoleConstants.ProjectLeader)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(RoleConstants.Guest)).GetAwaiter().GetResult();
                }

                // Since all user is default to user role, we add it for them
                await _userManager.AddToRoleAsync(user, RoleConstants.User);
                await _userStore.SetUserNameAsync(user, Input.Name, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                // Create a user with email and input password
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    // Add real user to database
                    await _userRepo.Add(new User
                    {
                        UserID = new Guid(user.Id), // The link between 2 user table should be this id
                        UserFullName = Input.Name,
                        UserEmail = Input.Email,
                        UserAvatar = MyConstants.DefaultAvatarUrl,
                    });

                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    // Where the email sending begins
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation",
                            new { email = Input.Email, returnUrl = returnUrl });
                    }

                    var businessUser = _userRepo.Get(u => u.UserID == user.Id);
                    HttpContext.Session.SetString("user", JsonConvert.SerializeObject(businessUser));
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    // TODO: Return to the home page
                    return RedirectToAction("Index", "EditUser");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private AuthUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<AuthUser>(); // Change this so that our custom user will be used
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                                                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                                                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
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