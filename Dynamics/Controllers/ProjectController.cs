using Dynamics.DataAccess.Repository;
using Dynamics.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Dynamics.Controllers
{
    public class ProjectController : Controller
    {
        IProjectRepository _projectRepository;

        public ProjectController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateProject()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(Project project, IFormFile image)
        {
            return RedirectToAction("Announce", "Home");
        }
    }
}
