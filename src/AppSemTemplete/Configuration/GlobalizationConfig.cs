using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace AppSemTemplate.Configuration
{
    public static class GlobalizationConfig
    {
        public static WebApplication UseGlobalizationConfig(this WebApplication app)
        {
            // Define a cultura padrão da aplicação
            var defaultCulture = new CultureInfo("pt-BR");

            // Define as opções de localização da aplicação
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(defaultCulture),
                SupportedCultures = new List<CultureInfo> { defaultCulture },
                SupportedUICultures = new List<CultureInfo> { defaultCulture }
            };

            // Adiciona o middleware de localização na aplicação, alterando o comportamento padrão que é definida pelo browser do request
            app.UseRequestLocalization(localizationOptions);

            return app;
        }
    }
}
