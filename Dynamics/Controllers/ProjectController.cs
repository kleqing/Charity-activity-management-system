using Dynamics.DataAccess.Repository;
using Dynamics.Helps;
using Dynamics.Models.Models;
using Dynamics.Models.Models.ViewModel;
using Dynamics.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace Dynamics.Controllers
{
    public class ProjectController : Controller
    {
        IProjectRepository _projectRepository;
        private readonly UserManager<IdentityUser> userManager;

        public ProjectController(IProjectRepository projectRepository, UserManager<IdentityUser> userManager)
        {
            _projectRepository = projectRepository;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateProject()
        {
            var currentOrganization = HttpContext.Session.Get<OrganizationVM>(MySettingSession.SESSION_Current_Organization_KEY);


            var projectVM = new ProjectVM()
            {
                ProjectID = Guid.NewGuid(),
                OrganizationID = currentOrganization.OrganizationID,
                ProjectStatus = 0,
                StartTime = DateOnly.FromDateTime(DateTime.UtcNow),
                OrganizationVM = currentOrganization,
            };
            return View(projectVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(ProjectVM projectVM, IFormFile image)
        {
            var currentOrganization = HttpContext.Session.Get<OrganizationVM>(MySettingSession.SESSION_Current_Organization_KEY);

            projectVM.OrganizationVM = currentOrganization;

            var Leader = new User();
            foreach(var item in currentOrganization.OrganizationMember)
            {
                if (item.UserID.Equals(projectVM.LeaderID))
                {
                    Leader = item.User;
                }
            }
            if (projectVM != null)
            {
                if (image != null)
                {
                    projectVM.Attachment = Utility.Util.UploadImage(image, @"images\Project", projectVM.ProjectID.ToString());
                }

                if (projectVM.ProjectEmail == null)
                {
                    projectVM.ProjectEmail = Leader.UserEmail;
                }
                if (projectVM.ProjectPhoneNumber == null) 
                {
                    projectVM.ProjectPhoneNumber = Leader.UserPhoneNumber;
                }
                if (projectVM.ProjectAddress == null)
                {
                    projectVM.ProjectAddress = Leader.UserAddress;
                }
                var project = new Project()
                {
                    ProjectID = projectVM.ProjectID,
                    OrganizationID = projectVM.OrganizationID,
                    RequestID = projectVM.RequestID,
                    ProjectName = projectVM.ProjectName,
                    ProjectEmail = projectVM.ProjectEmail,
                    ProjectPhoneNumber = projectVM.ProjectPhoneNumber,
                    ProjectAddress = projectVM.ProjectAddress,
                    ProjectStatus = projectVM.ProjectStatus,
                    Attachment = projectVM.Attachment,
                    ProjectDescription = projectVM.ProjectDescription,
                    StartTime = projectVM.StartTime,
                    EndTime = projectVM.EndTime,
                };
                if(await _projectRepository.AddProjectAsync(project))
                {
                    return RedirectToAction(nameof(AutoJoinProject), new { projectId = project.ProjectID, leaderId = projectVM.LeaderID });
                }       
            }
            
            return View(projectVM);

           
        }

        public async Task<IActionResult> AutoJoinProject(Guid projectId, Guid leaderId)
        {
            //get current user
            var userString = HttpContext.Session.GetString("user");
            User currentUser = null;
            if (userString != null)
            {
                currentUser = JsonConvert.DeserializeObject<User>(userString);
            }

            //join Project
            var projectMember = new ProjectMember()
            {
                UserID = currentUser.UserID,
                ProjectID = projectId,
                Status = 2,
            };
            await _projectRepository.AddProjectMemberAsync(projectMember);
            if (!currentUser.UserID.Equals(leaderId))
            {
                var projectMember1 = new ProjectMember()
                {
                    UserID = leaderId,
                    ProjectID = projectId,
                    Status = 2,
                };
                await _projectRepository.AddProjectMemberAsync(projectMember1);
            }
            var projectResource = new ProjectResource()
            {
                ProjectID = projectId,
                ResourceName = "Money",
                Quantity = 0,
                ExpectedQuantity = 0,
                Unit = "VND",
            };
            await _projectRepository.AddProjectResourceAsync(projectResource);

            var currentProject = await _projectRepository.GetProjectByProjectIDAsync(p => p.ProjectID.Equals(projectId));
            HttpContext.Session.Set<Project>(MySettingSession.SESSION_Current_Project_KEY, currentProject);
            return RedirectToAction(nameof(AddProjectResource));
        }
        [HttpGet]

        public async Task<IActionResult> DetailProject(Guid projectId)
        {
            var currentProject = await _projectRepository.GetProjectByProjectIDAsync(p => p.ProjectID.Equals(projectId));
            HttpContext.Session.Set<Project>(MySettingSession.SESSION_Current_Project_KEY, currentProject);
            return RedirectToAction(nameof(AddProjectResource));
        }

        public async Task<IActionResult> AddProjectResource()
        {
            var currentProject = HttpContext.Session.Get<Project>(MySettingSession.SESSION_Current_Project_KEY);

            var projectResource = new ProjectResource()
            {
                ProjectID = currentProject.ProjectID,
                Quantity = 0,
            };

            var projectResources =await _projectRepository.GetAllResourceByProjectIDAsync(pr => pr.ProjectID .Equals(currentProject.ProjectID));
            HttpContext.Session.Set<List<ProjectResource>>(MySettingSession.SESSION_Resources_In_A_PRoject_KEY, projectResources);

            return View(projectResource);
        }

        [HttpPost]
        public async Task<IActionResult> AddProjectResource(ProjectResource projectResource)
        {
            await _projectRepository.AddProjectResourceAsync(projectResource);
            return RedirectToAction(nameof(AddProjectResource));
        }
    }
}
