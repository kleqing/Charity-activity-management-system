using Dynamics.Areas.Admin.Models;
using Dynamics.Areas.Admin.Ultility;
using Dynamics.DataAccess.Repository;
using Dynamics.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dynamics.Areas.Admin.Controllers
{
    [Authorize(Roles = RoleConstants.Admin)]
    [Area("Admin")]
    public class HomeController : Controller
    {
        public readonly IAdminRepository _adminRepository;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(IAdminRepository adminRepository, SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
            _adminRepository = adminRepository;
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(RoleConstants.Admin))
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
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
