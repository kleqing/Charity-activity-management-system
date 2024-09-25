using Dynamics.DataAccess.Repository;
using Dynamics.Models.Models;
using Dynamics.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dynamics.Controllers
{
    [Authorize(Roles = RoleConstants.User)]
    public class EditUserController : Controller
    {
        IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public EditUserController(IUserRepository userRepo, UserManager<IdentityUser> userManager)
        {
            _userRepository = userRepo;
            _userManager = userManager;
        }

        // GET: Client/Users
        public async Task<IActionResult> Index()
        {
            // TODO: Make a real user page
            var users = await _userRepository.GetAllUsers();
            //get current user
            /* var currentUser = await _userManager.GetUserAsync(User);
             if (currentUser == null)
             {
                 return NotFound();
             }
             var user = await _userRepository.Get(u => u.email.Equals(currentUser.Email));
             ViewBag.CurrentUserId = user.userID;*/
            return View(users);
            //return View();
        }


        // GET: Client/Users/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var user = await _userRepository.Get(u => u.UserID.Equals(id));
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Client/Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Client/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            await _userRepository.Add(user);
            //return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(Index));
        }

        // GET: Client/Users/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var user = await _userRepository.Get(u => u.UserID.Equals(id));

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Client/Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User user, IFormFile image)
        {
            if (user != null)
            {
                if (image != null)
                {
                    user.UserAvatar = Util.UploadImage(image, @"images\User", user.UserID.ToString());
                    await _userRepository.Update(user);
                    // Update the session as well
                    HttpContext.Session.SetString("user", JsonConvert.SerializeObject(user));
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(user);
        }

    }
}