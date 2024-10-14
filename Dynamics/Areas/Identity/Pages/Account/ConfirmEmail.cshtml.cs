﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Dynamics.DataAccess.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Text;
using Dynamics.Utility;


namespace Dynamics.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserRepository _userRepo;
        private readonly SignInManager<IdentityUser> _signInManager;

        public ConfirmEmailModel(UserManager<IdentityUser> userManager, IUserRepository userRepo, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _userRepo = userRepo;
            _signInManager = signInManager;
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
            // Decode and get the result
            var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, decodedCode);
            if (result.Succeeded)
            {
                // Sign in
                await _signInManager.SignInAsync(user, isPersistent: false);
                // Session
                var businessUser = await _userRepo.GetAsync(u => u.UserID.ToString() == user.Id);
                HttpContext.Session.SetString("user", JsonConvert.SerializeObject(businessUser));
                if (User.IsInRole(RoleConstants.Admin) && result.Succeeded)
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToPage("/Error");
            }
        }

    }
}
