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

namespace Dynamics.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository _userRepo;
        private readonly IRequestRepository _requestRepo;
        private readonly IProjectRepository _projectRepo;
        private readonly IProjectResourceRepository _projectResourceRepo;
        private readonly IMapper _mapper;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, IUserRepository userRepo, IRequestRepository requestRepo,
            IProjectRepository projectRepo,IProjectResourceRepository projectResourceRepo, 
            IMapper mapper, SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _userRepo = userRepo;
            _requestRepo = requestRepo;
            _projectRepo = projectRepo;
            _projectResourceRepo = projectResourceRepo;
            _mapper = mapper;
            _signInManager = signInManager;
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


            List<Request> requests = await _requestRepo.GetAllRequestWithUsersAsync();
            List<Project> projects = await _projectRepo.GetAllAsync();
            // Map to view model for display
            List<ProjectOverviewDto> projectOverviewDtos = new List<ProjectOverviewDto>();
            foreach (var p in projects)
            {
                ProjectOverviewDto dto = await MapToProjectOverviewDto(p);
                projectOverviewDtos.Add(dto);
            }

            // TODO: For organization, wait for Tuan
            var result = new HomepageViewModel
            {
                Requests = requests,
                Projects = projectOverviewDtos
            };

            return View(result);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<ProjectOverviewDto> MapToProjectOverviewDto(Project p)
        {
            // TODO: Use mapper instead
            var tempProjectOverviewDto = _mapper.Map<ProjectOverviewDto>(p);
            //var tempProjectOverviewDto = new ProjectOverviewDto();
            tempProjectOverviewDto.ProjectName = p.ProjectName;
            tempProjectOverviewDto.ProjectId = p.ProjectID;
            tempProjectOverviewDto.ProjetDescription = p.ProjectDescription;
            tempProjectOverviewDto.ProjectLocation = "Not implemented";
            tempProjectOverviewDto.ProjectAttachment = p.Attachment;
            tempProjectOverviewDto.ProjectStatus = p.ProjectStatus;

            //tempProjectOverviewDto.ProjectUser = (await _userRepo.GetAsync(u => u.UserID == p.LeaderID)).UserFullName;
            tempProjectOverviewDto.ProjectMembers = await _projectRepo.CountMembersOfProjectByIdAsync(p.ProjectID);
            tempProjectOverviewDto.ProjectProgress = _projectRepo.GetProjectProgressById(p.ProjectID);
            var moneyRaised =
                await _projectResourceRepo.GetAsync(pr => pr.ResourceName == "Money" && pr.ProjectID == p.ProjectID);
            if (moneyRaised != null)
            {
                tempProjectOverviewDto.ProjectRaisedMoney = moneyRaised.Quantity ?? 0;
            }

            return tempProjectOverviewDto;
        }
    }
}