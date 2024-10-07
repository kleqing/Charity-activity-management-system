using Dynamics.Areas.Admin.Models;
using Dynamics.Areas.Admin.Ultility;
using Dynamics.DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Dynamics.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public readonly IAdminRepository _adminRepository;

        public HomeController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<IActionResult> Index()
        {
            // Get top 5 user and organization
            var top5User = await _adminRepository.GetTop5User();
            var top5Organization = await _adminRepository.GetTop5Organization();

            // View recent item (request)
            var recentRequest = await _adminRepository.ViewRecentItem();

            // Count user
            var userCount = await _adminRepository.CountUser();

            // Count organization
            var organizationCount = await _adminRepository.CountOrganization();


            // Count request
            var requestCount = await _adminRepository.CountRequest();


            // Count project
            var projectCount = await _adminRepository.CountProject();

            var model = new Dashboard
            {
                // Count of user, organization, request, project in database
                UserCount = userCount,
                OrganizationCount = organizationCount,
                RequestCount = requestCount,
                ProjectCount = projectCount,

                // View recent request (Recent item in dashoard page)
                GetRecentRequest = recentRequest,

                // List of top 5 user and organization
                TopUser = top5User,
                TopOrganization = top5Organization,
            };

            return View(model);
        }
    }
}
