using Dynamics.DataAccess.Repository;
using Dynamics.Models.Models;
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
            return RedirectToAction(nameof(AddProjectResource));
        }

        public async Task<IActionResult> AddProjectResource()
        {
            return View();
        }

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

            return RedirectToAction("ManageOrganizationMember", "Organization", new {projects = projects});
        }
    }
}
