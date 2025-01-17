using Microsoft.AspNetCore.Mvc;

namespace AppSemTemplete.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
