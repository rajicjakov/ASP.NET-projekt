using Microsoft.AspNetCore.Mvc;
using MVC_projekt.Data;
using MVC_projekt.Models;
using MVC_projekt.Services;

namespace MVC_projekt.Controllers
{
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(string.Empty, "Email and password are required.");
                return View();
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null || !PasswordHasher.Verify(password, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View();
            }

            TempData["LoginMessage"] = "Login successful. User was verified, but session support is not yet implemented.";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        public IActionResult Register(string username, string email, string password, string passwordConfirm)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(passwordConfirm))
            {
                ModelState.AddModelError(string.Empty, "All registration fields are required.");
                return View("Login");
            }

            if (password != passwordConfirm)
            {
                ModelState.AddModelError(string.Empty, "Password and confirmation do not match.");
                return View("Login");
            }

            if (_context.Users.Any(u => u.Email == email))
            {
                ModelState.AddModelError(string.Empty, "A user with this email already exists.");
                return View("Login");
            }

            if (_context.Users.Any(u => u.Username == username))
            {
                ModelState.AddModelError(string.Empty, "A user with this username already exists.");
                return View("Login");
            }

            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = PasswordHasher.Hash(password),
                DateJoined = DateTime.UtcNow,
                Role = UserRole.Regular
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["RegisterMessage"] = "Account created successfully. You may now log in.";
            return RedirectToAction("Login");
        }
    }
}