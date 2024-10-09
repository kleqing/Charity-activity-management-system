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
    public class ProjectsController : Controller
    {
        private readonly IAdminRepository _adminRepository = null;

        public ProjectsController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        // GET: Admin/Projects
        public async Task<IActionResult> Index(int? status)
        {
            if (User.IsInRole(RoleConstants.Admin))
            {
                var projects = await _adminRepository.ViewProjects();

                if (status.HasValue)
                {
                    projects = projects.Where(c => c.ProjectStatus == status.Value).ToList();
                }
                return View(projects);
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Admin/Projects/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (User.IsInRole(RoleConstants.Admin))
            {
                var project = await _adminRepository.GetProjects(c => c.ProjectID == id);
                return View(project);
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<JsonResult> ChangeStatus(Guid id)
        {
            var result = await _adminRepository.BanProject(id);
            var project = await _adminRepository.GetProjects(p => p.ProjectID == id);

            return Json(new
            {
                isBanned = result,
                projectStatus = project.ProjectStatus,

                endTime = project.EndTime?.ToString("MM/dd/yyyy")
            });
        }


    }
}
