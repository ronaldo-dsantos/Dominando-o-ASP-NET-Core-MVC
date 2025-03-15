using AppSemTemplate.Configuration;
using AppSemTemplate.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace AppSemTemplate.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly ApiConfiguration ApiConfiguration;
        private readonly ILogger<HomeController> Logger;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(IConfiguration configuration, 
                              IOptions<ApiConfiguration> apiConfiguration,
                              ILogger<HomeController> logger,
                              IStringLocalizer<HomeController> localizer)
                                
        {
            Configuration = configuration;
            ApiConfiguration = apiConfiguration.Value;
            Logger = logger;
            _localizer = localizer;
        }

        // Exemplo de como utilizar o ResponseCache (exemplo didático, na vida real, utilizar em situações que não dependem do usuário logado)
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult Index()
        {
            // Exemplo de como imprimir logs no ElmahIo
            Logger.LogInformation("Information");
            Logger.LogCritical("Critical");
            Logger.LogWarning("Warning");
            Logger.LogError("Error");

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

            // Adicionando msg que será exibida conforme o idioma selecionado
            ViewData["Message"] = _localizer["Seja bem vindo!"];

            ViewData["Horario"] = DateTime.Now;
 
            return View();
        }


        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }


        [Route("teste")]
        public IActionResult Teste()
        {
            throw new Exception("ALGO ERRADO NÃO ESTAVA CERTO!");

            return View("Index");
        }


        [Route("erro/{id:length(3,3)}")]
        public IActionResult Errors(int id)
        {
            var modelErro = new ErrorViewModel();

            if (id == 500)
            {
                modelErro.Mensagem = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
                modelErro.Titulo = "Ocorreu um erro!";
                modelErro.ErroCode = id;
            }
            else if (id == 404)
            {
                modelErro.Mensagem = "A página que está procurando não existe! <br />Em caso de dúvidas entre em contato com nosso suporte";
                modelErro.Titulo = "Ops! Página não encontrada.";
                modelErro.ErroCode = id;
            }
            else if (id == 403)
            {
                modelErro.Mensagem = "Você não tem permissão para fazer isto.";
                modelErro.Titulo = "Acesso Negado";
                modelErro.ErroCode = id;
            }
            else
            {
                return StatusCode(500);
            }

            return View("Error", modelErro);
        }
    }
}
