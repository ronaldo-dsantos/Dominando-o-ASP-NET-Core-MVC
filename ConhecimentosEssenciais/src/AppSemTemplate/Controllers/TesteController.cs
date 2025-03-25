using Microsoft.AspNetCore.Mvc;

namespace AppSemTemplate.Controllers
{
    // Controller para exemplo de teste
    public class TesteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
