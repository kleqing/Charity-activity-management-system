using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CharityActivityWebApplication.Areas.Client.Controllers
{
	[Area("Client")]
	[Authorize(Roles = "Client")]
	[Authorize(AuthenticationSchemes = "Client")]
	public class HomeController : Controller
    {
        // GET: HomeController
        public ActionResult Index()
        {
            return View();
        }
    }
}
