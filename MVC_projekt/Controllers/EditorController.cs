using Microsoft.AspNetCore.Mvc;

namespace MVC_projekt.Controllers
{
    public class EditorController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }
    }
}