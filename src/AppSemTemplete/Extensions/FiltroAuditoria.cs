using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AppSemTemplate.Extensions
{
    public class FiltroAuditoria : IActionFilter
    {
        private readonly ILogger<FiltroAuditoria> _logger;

        public FiltroAuditoria(ILogger<FiltroAuditoria> logger)
        {
            _logger = logger;
        }

        // Exemplo de como trabalhar com filtros de ação
        // Filtro para auditoria capturando o que foi executado após a execução da ação
        public void OnActionExecuted(ActionExecutedContext context)
        {         
            if (context.HttpContext.User?.Identity?.IsAuthenticated == true)
            {
                var message = $"{context.HttpContext.User.Identity.Name} Acessou: {context.HttpContext.Request.GetDisplayUrl()}";

                //Gravando o log, o correto seria gravar como LogInformation, mas como definimos para gravar log a partir de Warning, então vamos gravar como Warning para fins didáticos
                _logger.LogWarning(message);
            }            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Implementar a lógica de auditoria para o que está sendo executado no momento
        }
    }
}
