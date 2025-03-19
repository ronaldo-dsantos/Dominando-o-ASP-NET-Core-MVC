using Elmah.Io.AspNetCore;
using Elmah.Io.Extensions.Logging;

namespace AppSemTemplate.Configuration
{
    public static class LoggingConfig
    {
        public static WebApplicationBuilder AddElmahConfiguration(this WebApplicationBuilder builder)
        {
            // Adicionando configurações do ElmahIo
            builder.Services.Configure<ElmahIoOptions>(builder.Configuration.GetSection("ElmahIo"));
            builder.Services.AddElmahIo();

            // Adicionando configurações do ElmahIo para capturar os logs padrão do aspnet core e enviar para o ElmahIo
            builder.Logging.Services.Configure<ElmahIoProviderOptions>(builder.Configuration.GetSection("ElmahIo"));
            builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
            builder.Logging.AddElmahIo();

            // Adicionando níveis de log para o ElmahIo, nesse caso, só será logado os logs de Warning para cima
            builder.Logging.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Warning);

            return builder;
        }
    }
}
