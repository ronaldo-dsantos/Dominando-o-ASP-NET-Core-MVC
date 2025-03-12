using AppSemTemplate.Services;

namespace AppSemTemplate.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static WebApplicationBuilder AddDependencyInjectionConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Tipos de ciclo de vida para injeção de dependência
            builder.Services.AddTransient<IOperacaoTransient, Operacao>();
            builder.Services.AddScoped<IOperacaoScoped, Operacao>();
            builder.Services.AddSingleton<IOperacaoSingleton, Operacao>();
            builder.Services.AddSingleton<IOperacaoSingletonInstance>(new Operacao(Guid.Empty));

            builder.Services.AddTransient<OperacaoService>();

            return builder;
        }
    }
}
