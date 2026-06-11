using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_projekt.Models;
using System.Security.Claims;

namespace MVC_projekt.Controllers
{
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public LoginController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("")]
        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, bool rememberMe = false)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(string.Empty, "Email and password are required.");
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, rememberMe, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string username, string email, string password, string passwordConfirm)
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

            if (await _userManager.FindByEmailAsync(email) != null)
            {
                ModelState.AddModelError(string.Empty, "A user with this email already exists.");
                return View("Login");
            }

            if (await _userManager.FindByNameAsync(username) != null)
            {
                ModelState.AddModelError(string.Empty, "A user with this username already exists.");
                return View("Login");
            }

            var user = new User
            {
                UserName = username,
                Email = email,
                DateJoined = DateTime.UtcNow,
                Role = UserRole.Regular
            };

            var createResult = await _userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
            {
                foreach (var error in createResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View("Login");
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost("Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost("ExternalLogin")]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Login", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet("ExternalLoginCallback")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"External provider error: {remoteError}");
                return View("Login");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction("Login");
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (signInResult.Succeeded)
            {
                return RedirectToLocal(returnUrl);
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (email == null)
            {
                ModelState.AddModelError(string.Empty, "Email claim not received from external provider.");
                return View("Login");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new User
                {
                    UserName = email.Split('@')[0],
                    Email = email,
                    DateJoined = DateTime.UtcNow,
                    Role = UserRole.Regular
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Error creating user account.");
                    return View("Login");
                }
            }

            var addLoginResult = await _userManager.AddLoginAsync(user, info);
            if (!addLoginResult.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Error adding external login.");
                return View("Login");
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToLocal(returnUrl);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
