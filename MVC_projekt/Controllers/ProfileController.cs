using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_projekt.Models;
using System.IO;

namespace MVC_projekt.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _env;

        public ProfileController(UserManager<User> userManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _env = env;
            // ensure profiles upload folder exists immediately
            try
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath ?? string.Empty, "images", "profiles");
                Directory.CreateDirectory(uploadsFolder);
            }
            catch
            {
                // ignore errors creating folder
            }
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var photoUrl = "/lib/bootstrap/icons/person-circle.svg";
            var uploadsFolder = Path.Combine(_env.WebRootPath ?? string.Empty, "images", "profiles");
            if (Directory.Exists(uploadsFolder))
            {
                var files = Directory.GetFiles(uploadsFolder, user.Id + ".*");
                if (files.Length > 0)
                {
                    photoUrl = "/images/profiles/" + Path.GetFileName(files[0]);
                }
            }

            var model = new ProfileViewModel { User = user, PhotoUrl = photoUrl };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhoto(IFormFile file)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();
            if (file == null || file.Length == 0) return BadRequest();

            var uploadsFolder = Path.Combine(_env.WebRootPath ?? string.Empty, "images", "profiles");
            Directory.CreateDirectory(uploadsFolder);

            var ext = Path.GetExtension(file.FileName);
            var fileName = user.Id + ext;

            // remove existing files for this user
            var existing = Directory.GetFiles(uploadsFolder, user.Id + ".*");
            foreach (var f in existing)
            {
                try { System.IO.File.Delete(f); } catch { }
            }

            var filePath = Path.Combine(uploadsFolder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var photoUrl = "/images/profiles/" + fileName;
            return Json(new { success = true, url = photoUrl });
        }
    }
}
