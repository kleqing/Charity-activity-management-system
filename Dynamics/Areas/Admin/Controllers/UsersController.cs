﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dynamics.DataAccess;
using Dynamics.Models.Models;
using Dynamics.DataAccess.Repository;

namespace Dynamics.Areas.Admin.Controllers
{
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
            var users = await _adminRepository.ViewUser();
            return View(users);
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
    }
}
