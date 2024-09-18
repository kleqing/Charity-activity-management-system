using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Charity_Management_System;
using Repository;

namespace CharityActivityWebApplication.Areas.Client.Controllers
{
    [Area("Client")]
    public class UsersController : Controller
    {
        IUserRepository _userRepository = null;
        public UsersController()
        {
           _userRepository = new UserRepository();
        }

        // GET: Client/Users
        public async Task<IActionResult> Index()
        {
            var users = _userRepository.GetAllUsers();
            return View(users);
        }

        // GET: Client/Users/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userRepository.GetUserById(id);
			if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Client/Users/Create
        public async Task<IActionResult> Create()
        {
            ViewData["userID"] = new SelectList(await _userRepository.GetAllUsers(), "transactionID", "message");
            return View();
        }

        // POST: Client/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("userID,name,dateOfBirth,email,phoneNumber,address,password,roleID,awardID,avatar,description")] User user)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(user.password))
				{
                    return View(user);
				}
				await _userRepository.AddUser(user);
				return RedirectToAction(nameof(Index));
            }
            ViewData["userID"] = new SelectList(await _userRepository.GetAllUsers(), "transactionID", "message", user.userID);
            return View(user);
        }

        // GET: Client/Users/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userRepository.GetUserById(Convert.ToInt32(id));

			if (user == null)
            {
                return NotFound();
            }
            ViewData["userID"] = new SelectList(await _userRepository.GetAllUsers(), "transactionID", "message", user.userID);
            return View(user);
        }

        // POST: Client/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("userID,name,dateOfBirth,email,phoneNumber,address,password,roleID,awardID,avatar,description")] User user)
        {
            if (ModelState.IsValid)
            {
                await _userRepository.UpdateUser(user);
				return RedirectToAction(nameof(Index));
            }
            ViewData["userID"] = new SelectList(await _userRepository.GetAllUsers(), "transactionID", "message", user.userID);
            return View(user);
        }

        // GET: Client/Users/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userRepository.GetUserById(Convert.ToInt32(id));
            
            if (user == null)
            {
                return NotFound();
            }
            await _userRepository.DeleteUser(id);
			return View(user);
        }

        // POST: Client/Users/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var user = await _context.Users.FindAsync(id);
        //    if (user != null)
        //    {
        //        _context.Users.Remove(user);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool UserExists(int id)
        //{
        //    return _context.Users.Any(e => e.userID == id);
        //}
    }
}
