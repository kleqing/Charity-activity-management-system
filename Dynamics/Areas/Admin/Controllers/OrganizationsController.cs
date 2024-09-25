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

namespace Dynamics.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrganizationsController : Controller
    {
        private readonly IAdminRepository _adminRepository;

        public OrganizationsController(AdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        // GET: Admin/Organizations
        public async Task<IActionResult> Index()
        {
            return View(await _adminRepository.ViewOrganization());
        }
    }
}
