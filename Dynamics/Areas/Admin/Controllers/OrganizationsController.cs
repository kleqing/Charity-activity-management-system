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
using Microsoft.AspNetCore.Authorization;

namespace Dynamics.Areas.Admin.Controllers
{
    [Authorize(Roles = RoleConstants.Admin)]
    [Area("Admin")]
    public class OrganizationsController : Controller
    {
        private readonly IAdminRepository _adminRepository;

        public OrganizationsController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        // GET: Admin/Organizations
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(RoleConstants.Admin))
            {
                return View(await _adminRepository.ViewOrganization());
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetOrganizationInfo(Guid id)
        {
            var organization = await _adminRepository.GetOrganizationInfomation(o => o.OrganizationID == id);
            if (organization == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Organization not found"
                });
            }

            var memberCount = await _adminRepository.MemberJoinedOrganization(id);
            return Json(new
            {
                success = true,
                data = new
                {
                    organization.OrganizationName,
                    organization.OrganizationDescription,
                    organization.OrganizationEmail,
                    organization.OrganizationPhoneNumber,
                    organization.OrganizationAddress,
                    organization.StartTime,
                    organization.ShutdownDay,
                    memberCount = memberCount
                }
            });
        }

        [HttpPost]
        public async Task<JsonResult> ChangeStatus(Guid id)
        {
            var result = await _adminRepository.ChnageOrganizationStatus(id);
            return Json(new
            {
                Status = result
            });
        }
    }
}
