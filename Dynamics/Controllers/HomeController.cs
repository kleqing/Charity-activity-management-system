using AutoMapper;
using Dynamics.DataAccess.Repository;
using Dynamics.Models;
using Dynamics.Models.Models;
using Dynamics.Models.Models.Dto;
using Dynamics.Models.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;
using Dynamics.Services;
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
            var unfinishedProjects = new List<ProjectOverviewDto>();
            var finishedProjects = new List<ProjectOverviewDto>();
            // Map to project overview dto
            foreach (var p in projects)
            {
                var dto = _projectService.MapToProjectOverviewDto(p);
                if (dto.ProjectStatus <= 1)
                {
                    unfinishedProjects.Add(dto);
                }
                else
                {
                    finishedProjects.Add(dto);
                }
            }

            var result = new HomepageViewModel
            {
                Requests = requestOverview,
                OnGoingProjects = unfinishedProjects,
                SuccessfulProjects = finishedProjects,
                Organizations = orgsOverview,
            };
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            string[] args = query.Split("-");
            // Args < 2 search all
            if (args.Length < 2)
            {
                var requests = await _requestRepo.GetAllAsync(request =>
                    request.RequestTitle.Contains(query, StringComparison.OrdinalIgnoreCase));
                var projects = await _projectRepo.GetAllAsync(prj =>
                    prj.ProjectName.Contains(query, StringComparison.OrdinalIgnoreCase));
                var organizations =
                    await _organizationRepo.GetAllOrganizationsWithExpressionAsync(organization =>
                        organization.OrganizationName.Contains(query, StringComparison.OrdinalIgnoreCase));

                var requestOverviewDtos = _requestService.MapToListRequestOverviewDto(requests);
                var projectOverviewDtos = _projectService.MapToListProjectOverviewDto(projects);
                var organizationOverviewDtos = _organizationService.MapToOrganizationOverviewDtoList(organizations);

                return View(new HomepageViewModel
                {
                    Requests = requestOverviewDtos,
                    OnGoingProjects = projectOverviewDtos,
                    Organizations = organizationOverviewDtos
                });
            }

            // Only search by a specific type
            var type = args[0];
            var target = args[1];
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}