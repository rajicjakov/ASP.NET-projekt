using Microsoft.AspNetCore.Mvc;

namespace MVC_projekt.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}