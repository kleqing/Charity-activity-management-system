using Dynamics.DataAccess.Repository;
using Dynamics.Models;
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

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated != null && User.Identity.IsAuthenticated)
            {
                RedirectToAction("Index", "EditUser");
            }
            return View();
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
