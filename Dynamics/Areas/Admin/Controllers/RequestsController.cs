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
using Microsoft.AspNetCore.Authorization;
using Dynamics.Utility;

namespace Dynamics.Areas.Admin.Controllers
{
    [Authorize(Roles = RoleConstants.Admin)]
    [Area("Admin")]
    public class RequestsController : Controller
    {
        private readonly IAdminRepository _adminRepository;

        public RequestsController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        // GET: Admin/Requests
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(RoleConstants.Admin))
            {
                return View(await _adminRepository.ViewRequest());
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<JsonResult> ChangeStatus(Guid id, int status)
        {
            var result = await _adminRepository.ChangeRequestStatus(id);
            return Json(new
            {
                status = result
            });
        }

        // Delete request
        [HttpPost]
        public async Task<JsonResult> Delete(Guid id)
        {
            var request = await _adminRepository.DeleteRequest(id);
            if (request == null)
            {
                return Json(new { success = false });
            }

            return Json(new { success = true });
        }

    }
}
