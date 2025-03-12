using AppSemTemplate.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AppSemTemplete.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration Configuration;

        private readonly ApiConfiguration ApiConfiguration;

        public HomeController(IConfiguration configuration, 
                              IOptions<ApiConfiguration> apiConfiguration)
        {
            Configuration = configuration;
            ApiConfiguration = apiConfiguration.Value;

        }

        public IActionResult Index()
        {
            // Exemplo de como capturar o ambiente da aplicação
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Exemplo de como capturar configurações do appsettings.json
            var apiConfig = new ApiConfiguration();
            Configuration.GetSection("ApiConfiguration").Bind(apiConfig);
                        
            var secret = apiConfig.UserSecret;

            // Maneira de capturar configurações do appsettings.json na mão sem a necessidade de criar uma classe
            var user = Configuration["ApiConfiguration:UserKey"];

            // Maneira de capturar uma configurações do appsettings.json que foi disponibilizada globalmente através da classe program (melhor opção)
            var domain = ApiConfiguration.Domain;

            return View();
        }
    }
}
