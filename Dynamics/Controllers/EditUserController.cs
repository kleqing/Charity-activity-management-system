using Dynamics.DataAccess.Repository;
using Dynamics.Helps;
using Dynamics.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dynamics.Controllers
{
    public class EditUserController : Controller
    {
        IUserRepository _userRepository = null;
        public EditUserController(IUserRepository userRepo)
        {
            _userRepository = userRepo;
        }

        // GET: Client/Users
        public async Task<IActionResult> Index()
        {
            // TODO: Make a real user page
            var users = await _userRepository.GetAllUsers();
            return View(users);
            //return View();
        }

        // GET: Client/Users/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userRepository.Get(u => u.userID == id);
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
        public async Task<IActionResult> Edit(int? id)
        {
            var user = await _userRepository.Get(u => u.userID == Convert.ToInt32(id));

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Client/Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User user, IFormFile Image)
        {
            if (user != null)
            {
                if(Image != null)
                {
                    user.avatar = Util.UploadImage(Image, "User");
                }
                await _userRepository.Update(user);
                return RedirectToAction(nameof(Index));
            }
            ViewData["userID"] = new SelectList(await _userRepository.GetAllUsers(), "transactionID", "message", user.userID);
            return View(user);
        }

        // GET: Client/Users/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userRepository.Get(u => u.userID == Convert.ToInt32(id));

            if (user == null)
            {
                return NotFound();
            }
            await _userRepository.Get(u => u.userID == id);
            return RedirectToAction(nameof(Index));
        }
    }
}
