using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace AppSemTemplate.Configuration
{
    public static class GlobalizationConfig
    {
        //public static WebApplication UseGlobalizationConfig(this WebApplication app)
        //{
        //    // Define a cultura padrão da aplicação
        //    var defaultCulture = new CultureInfo("pt-BR");

        //    // Define as opções de localização da aplicação
        //    var localizationOptions = new RequestLocalizationOptions
        //    {
        //        DefaultRequestCulture = new RequestCulture(defaultCulture),
        //        SupportedCultures = new List<CultureInfo> { defaultCulture },
        //        SupportedUICultures = new List<CultureInfo> { defaultCulture }
        //    };

        //    // Adiciona o middleware de localização na aplicação, alterando o comportamento padrão que é definida pelo browser do request
        //    app.UseRequestLocalization(localizationOptions);

        //    return app;
        //}

        // Refatorando o método UseGlobalizationConfig para utilizar as opções de localização configuradas no método AddGlobalizationConfig
        public static WebApplication UseGlobalizationConfig(this WebApplication app)
        {
            var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizationOptions.Value);

            return app;
        }

        // Adicionando suporte a mais de uma cultura na aplicação
        public static WebApplicationBuilder AddGlobalizationConfig(this WebApplicationBuilder builder)
        {
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources"); // Informando o caminho onde estão os arquivos de recursos de acordo com a cultura

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { "pt-BR", "en-US" };
                options.SetDefaultCulture(supportedCultures[0])
                    .AddSupportedCultures(supportedCultures)
                    .AddSupportedUICultures(supportedCultures);
            });

            return builder;
        }
    }
}
