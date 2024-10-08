using AutoMapper;
using Dynamics.DataAccess.Repository;
using Dynamics.Models;
using Dynamics.Models.Models;
using Dynamics.Models.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;
using Dynamics.Services;
using Dynamics.Models.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Dynamics.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepo;
        private readonly IRequestRepository _requestRepo;
        private readonly IProjectRepository _projectRepo;
        private readonly IProjectService _projectService;
        private readonly IRequestService _requestService;
        private readonly IOrganizationRepository _organizationRepo;
        private readonly IOrganizationService _organizationService;

        public HomeController(IUserRepository userRepo, IRequestRepository requestRepo,
            IProjectRepository projectRepo, IProjectService projectService, IRequestService requestService,
            IOrganizationRepository organizationRepo, IOrganizationService organizationService)
        {
            _userRepo = userRepo;
            _requestRepo = requestRepo;
            _projectRepo = projectRepo;
            _projectService = projectService;
            _requestService = requestService;
            _organizationRepo = organizationRepo;
            _organizationService = organizationService;
        }

        // Landing page
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Homepage()
        {
            // Check if there is an authenticated user, set the session of that user
            if (User.Identity.IsAuthenticated)
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var user = _userRepo.GetAsync(u => u.UserEmail == userEmail).Result;
                // Bad user
                if (user == null) return RedirectToAction("Logout", "Auth");
                HttpContext.Session.SetString("user", JsonConvert.SerializeObject(user));
            }

            var requests = await _requestRepo.GetRequestsAsync();
            var requestOverview = _requestService.MapToListRequestOverviewDto(requests);

            var orgs = await _organizationRepo.GetAll().ToListAsync();
            var orgsOverview = _organizationService.MapToOrganizationOverviewDtoList(orgs);

            List<Project> projects = await _projectRepo.GetAllAsync();
            var projectDtos = _projectService.MapToListProjectOverviewDto(projects);

            var result = new HomepageViewModel
            {
                Requests = requestOverview,
                Projects = projectDtos,
                Organizations = orgsOverview,
            };
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string? query)
        {
            if (query == null) return RedirectToAction(nameof(Homepage));
            string[] args = query.Split("-");
            // Args < 2 search all
            if (args.Length < 2)
            {
                var requests = await _requestRepo.GetAllAsync();
                dynamic targets = requests
                    .Where(r => r.RequestTitle.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
                var requestOverviewDtos = _requestService.MapToListRequestOverviewDto(targets);

                var projects = await _projectRepo.GetAllAsync();
                targets = projects.Where(r => r.ProjectName.Contains(query, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                var projectOverviewDtos = _projectService.MapToListProjectOverviewDto(targets);

                var organizations =
                    await _organizationRepo.GetAllOrganizationsWithExpressionAsync();
                targets = organizations
                    .Where(r => r.OrganizationName.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
                var organizationOverviewDtos = _organizationService.MapToOrganizationOverviewDtoList(targets);

                return View(new HomepageViewModel
                {
                    Requests = requestOverviewDtos,
                    Projects = projectOverviewDtos,
                    Organizations = organizationOverviewDtos
                });
            }
            else
            {
                // Only search by a specific type
                var type = args[0];
                var target = args[1];

                if (type.Contains("req"))
                {
                    var requests = await _requestRepo.GetAllAsync();
                    var targets = requests
                        .Where(r => r.RequestTitle.Contains(target, StringComparison.OrdinalIgnoreCase)).ToList();
                    var requestOverviewDtos = _requestService.MapToListRequestOverviewDto(targets);
                    return View(new HomepageViewModel
                    {
                        Requests = requestOverviewDtos,
                    });
                }

                if (type.Contains("prj"))
                {
                    var projects = await _projectRepo.GetAllAsync();
                    var targets = projects.Where(r => r.ProjectName.Contains(target, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    var projectOverviewDtos = _projectService.MapToListProjectOverviewDto(targets);
                    return View(new HomepageViewModel
                    {
                        Projects = projectOverviewDtos,
                    });
                }

                if (type.Contains("org"))
                {
                    var organizations =
                        await _organizationRepo.GetAllOrganizationsWithExpressionAsync();
                    var targets = organizations
                        .Where(r => r.OrganizationName.Contains(target, StringComparison.OrdinalIgnoreCase)).ToList();
                    var organizationOverviewDtos = _organizationService.MapToOrganizationOverviewDtoList(targets);
                    return View(new HomepageViewModel
                    {
                        Organizations = organizationOverviewDtos,
                    });
                }
            }

            // if we get here something went wrong
            return RedirectToAction(nameof(Homepage));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Announce()
        {
            return View();
        }
    }
}