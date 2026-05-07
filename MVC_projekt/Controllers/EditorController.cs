using Microsoft.AspNetCore.Mvc;

namespace MVC_projekt.Controllers
{
    [Route("[controller]")]
    public class EditorController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }
    }
}