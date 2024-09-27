using Dynamics.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dynamics.Controllers;

// For testing purposes
// [Authorize(Roles = RoleConstants.Admin)]
public class SecretController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}