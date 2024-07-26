using Microsoft.AspNetCore.Mvc;

namespace Obligatorio2.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
