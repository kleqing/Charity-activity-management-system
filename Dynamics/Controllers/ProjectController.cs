using Dynamics.DataAccess.Repository;
using Dynamics.Helps;
using Dynamics.Models.Models;
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
            return View(new Project());
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(Project project, IFormFile image)
        {
            if (project != null)
            {
                if (image != null)
                {
                    project.Attachment = Utility.Util.UploadImage(image, @"images\Project", project.LeaderID);
                }
                await _projectRepository.AddProjectAsync(project);

                return RedirectToAction(nameof(AutoJoinProject), new { projectId = project.ProjectID, leaderId = project.LeaderID});
            }
            
            return View(project);

           
        }

        public async Task<IActionResult> AutoJoinProject(int projectId, string leaderId)
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
            return RedirectToAction(nameof(AddProjectResource));
        }

        public async Task<IActionResult> AddProjectResource()
        {
            return View();
        }


        //send for OrganizationProject Page
        public async Task<IActionResult> ManageOrganizationProjectByOrganizationID()
        {
            //current Organization
            var organizationString = HttpContext.Session.GetString("organization");
            Organization currentOrganization = null;
            if (organizationString != null)
            {
                currentOrganization = JsonConvert.DeserializeObject<Organization>(organizationString);
            }

            //find list project is manage by a Organization
            var projects = await _projectRepository.GetAllProjectsByOrganizationIDAsync(p => p.OrganizationID == currentOrganization.OrganizationID);

            //Create session for list project of a organization
            HttpContext.Session.Set<List<Project>>(MySettingSession.SESSION_Projects_In_A_OrganizationID_Key, projects);

            return RedirectToAction("ManageOrganizationProject", "Organization");
        }
    }
}
