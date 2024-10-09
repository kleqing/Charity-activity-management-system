using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dynamics.DataAccess;
using Dynamics.Models.Models;
using Dynamics.DataAccess.Repository;
using Dynamics.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Dynamics.Areas.Admin.Controllers
{
    [Authorize(Roles = RoleConstants.Admin)]
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IAdminRepository _adminRepository = null;

        public UsersController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        // GET: Admin/Users
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(RoleConstants.Admin))
            {
                var users = await _adminRepository.ViewUser();
                return View(users);
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<JsonResult> BanUser(Guid id)
        {
            var result = await _adminRepository.BanUserById(id);
            return Json(new
            {
                isBanned = result
            });
        }

        [HttpPost]
        public async Task<JsonResult> UserAsAdmin(Guid id)
        {
            await _adminRepository.ChangeUserRole(id);

            var userRole = await _adminRepository.GetUserRole(id);

            return Json(new
            {
                isAdmin = userRole == RoleConstants.Admin
            });
        }



    }
}
