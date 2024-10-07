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
            return View(await _adminRepository.ViewRequest());
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
