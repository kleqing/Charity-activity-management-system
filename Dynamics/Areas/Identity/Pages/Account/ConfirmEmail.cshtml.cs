// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Dynamics.DataAccess.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Text;

namespace Dynamics.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserRepository _userRepo;

        public ConfirmEmailModel(UserManager<IdentityUser> userManager, IUserRepository userRepo)
        {
            _userManager = userManager;
            _userRepo = userRepo;
        }

        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGetAsync(string userId, string code, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }
            // Decoed and get the result
            var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, decodedCode);
            if (result.Succeeded)
            {
                // Session
                var businessUser = await _userRepo.Get(u => u.UserID == user.Id);
                HttpContext.Session.SetString("user", JsonConvert.SerializeObject(businessUser));
                //return RedirectToPage("EmailConfirmationSuccess", new { returnUrl });
                return RedirectToAction("Index", "EditUser");
            }
            else
            {
                return RedirectToPage("/Error");
            }
        }

    }
}
