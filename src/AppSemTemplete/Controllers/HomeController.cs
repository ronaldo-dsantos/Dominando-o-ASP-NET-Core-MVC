using Microsoft.AspNetCore.Mvc;

namespace AppSemTemplete.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Exemplo de como capturar o ambiente da aplicação
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            return View();
        }
    }
}
