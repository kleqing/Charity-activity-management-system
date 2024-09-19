using Dynamics.DataAccess.Repository;
using Dynamics.Models;
using Dynamics.Models.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Dynamics.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository userRepo;

        public HomeController(ILogger<HomeController> logger, IUserRepository userRepo)
        {
            _logger = logger;
            this.userRepo = userRepo;
        }

        public async Task<IActionResult> Index()
        {
            // Testing
            List<User> user = await userRepo.GetAllUsers();
            return View(user);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
