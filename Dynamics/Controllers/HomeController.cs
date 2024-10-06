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

namespace Dynamics.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepo;
        private readonly IRequestRepository _requestRepo;
        private readonly IProjectRepository _projectRepo;
        private readonly IProjectService _projectService;

        public HomeController(IUserRepository userRepo, IRequestRepository requestRepo,
            IProjectRepository projectRepo, IProjectService projectService)
        {
            _userRepo = userRepo;
            _requestRepo = requestRepo;
            _projectRepo = projectRepo;
            _projectService = projectService;
        }

        // Landing page
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Homepage()
        {
            ModelState.AddModelError("Not found", "ABCXYZ");
            // Check if there is an authenticated user, set the session of that user
            if (User.Identity.IsAuthenticated)
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var user = _userRepo.GetAsync(u => u.UserEmail == userEmail).Result;
                // Bad user
                if (user == null)
                {
                    return RedirectToAction("Logout", "Auth");
                }

                HttpContext.Session.SetString("user", JsonConvert.SerializeObject(user));
            }

            List<Request> requests = await _requestRepo.GetRequestsAsync();
            List<Project> projects = await _projectRepo.GetAllAsync();
            
            // Map to view model for display
            var unfinishedProjects = new List<ProjectOverviewDto>();
            var finishedProjects = new List<ProjectOverviewDto>();
            foreach (var p in projects)
            {
                var dto =  _projectService.MapToProjectOverviewDto(p);
                if (dto.ProjectStatus <= 1)
                {
                    unfinishedProjects.Add(dto);
                }
                else
                {
                    finishedProjects.Add(dto);
                }
            }

            // TODO: For organization, wait for Tuan
            var result = new HomepageViewModel
            {
                Requests = requests,
                OnGoingProjects = unfinishedProjects,
                SuccessfulProjects = finishedProjects,
            };
            return View(result);
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