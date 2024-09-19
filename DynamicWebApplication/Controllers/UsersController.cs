using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BussinessObject;
using Repository;
using System.Security.Claims;

namespace DynamicWebApplication.Controllers
{
    public class UsersController : Controller
    {
        IUserRepository _userRepository = null;
        private readonly IWebHostEnvironment webHostEnvironment;

        public UsersController(IWebHostEnvironment webHostEnvironment)
        {
            _userRepository = new UserRepository();
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllUsers();
            return View(users);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("userID,userName,userDOB,userEmail,userPhoneNumber,userAddress,userPassword,userRoleID,userAvatarURL,userAvatar,userDescription")] User user)
        {
            if (user.userAvatar != null)
            {
                string fileName = UploadedFile(user);
                user.userAvatarURL = fileName;
            }
            await _userRepository.AddUser(user);
            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var user = await _userRepository.GetUserById(Convert.ToInt32(id));

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("userID,userName,userDOB,userEmail,userPhoneNumber,userAddress,userPassword,userRoleID,userAvatarURL,userAvatar,userDescription")] User user)
        {
            if (user != null)
            {
                if (user.userAvatar != null)
                {
                    string fileName = UploadedFile(user);
                    user.userAvatarURL = fileName;
                }
                await _userRepository.UpdateUser(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userRepository.GetUserById(Convert.ToInt32(id));

            if (user == null)
            {
                return NotFound();
            }
            await _userRepository.DeleteUser(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Users/Delete/5
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

        public string UploadedFile(User user)
        {
            string wwwRootPath = this.webHostEnvironment.WebRootPath;
            var exe = user.userAvatar.FileName;
            string fileName = Path.GetFileNameWithoutExtension(exe);
            string extension = Path.GetExtension(user.userAvatar.FileName);
            user.userAvatarURL = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string path = Path.Combine(wwwRootPath + "/upload/images", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                user.userAvatar.CopyTo(fileStream);
            }
            ViewBag.Avatar = user.userAvatarURL;
            return fileName;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile upload)
        {
            if (upload != null && upload.Length > 0)
            {
                // Get the current date and format it
                var currentDate = DateTime.Now;
                var year = currentDate.Year.ToString();
                var month = currentDate.Month.ToString().PadLeft(2, '0');
                var day = currentDate.Day.ToString().PadLeft(2, '0');

                // Create the directory string and ensure the directory exists
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload/images", year, month);
                Directory.CreateDirectory(directoryPath); // Creates all directories on the path if not exist

                // Modify the filename to include the date
                var fileName = $"{year}{month}{day}_{Path.GetFileName(upload.FileName)}";
                var filePath = Path.Combine(directoryPath, fileName);

                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await upload.CopyToAsync(stream);
                }

                // Return the JSON result with the URL to the uploaded file
                return Json(new { uploaded = true, url = $"/upload/images/{year}/{month}/{fileName}" });
            }

            return Json(new { uploaded = false, message = "Upload failed" });
        }
    }
}
