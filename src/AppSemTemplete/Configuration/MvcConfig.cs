using AppSemTemplate.Data;
using AppSemTemplate.Extensions;
using AppSemTemplate.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AppSemTemplate.Configuration
{
    public static class MvcConfig
    {
        public static WebApplicationBuilder AddMvcConfiguration(this WebApplicationBuilder builder)
        {
            // Reforçando o comportamento padrão para os arquivos de configuração de ambiente
            builder.Configuration
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables()
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true);

            // Aplicando globalmente o ValidateAntiForgeryToken em todas as requisições (pode ser aplicado individualmente em cada m�todo ou aplicado globalmente para toda a aplica��o) 
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                options.Filters.Add(typeof(FiltroAuditoria));

                // Adicionando mensagens de validação de ModelState customizadas
                MvcOptionsConfig.ConfigurarMensagensDeModelBinding(options.ModelBindingMessageProvider);
            });

            //Adicionando suporte a mudança de convenção de rota das áreas
            builder.Services.Configure<RazorViewEngineOptions>(options =>
            {
                options.AreaViewLocationFormats.Clear();
                options.AreaViewLocationFormats.Add("/Modulos/{2}/Views/{1}/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Modulos/{2}/Views/Shared/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
            });

            // Exemplo usando transformador de rota
            //builder.Services.AddRouting(options =>
            //{
            //    options.ConstraintMap["slugify"] = typeof(RouteSlugifyParamteterTrasformer);
            //});

            builder.Services.AddDbContext<AppDbContext>(o =>
                o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Alterando configura��es de HSTS
            builder.Services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
                options.ExcludedHosts.Add("example.com");
                options.ExcludedHosts.Add("www.example.com");
            });

            // Exemplo de como capturar configurações do appsettings.json e disponibilizar para a aplicação globalmente
            builder.Services.Configure<ApiConfiguration>(
                builder.Configuration.GetSection(ApiConfiguration.ConfigName));

            return builder;
        }

        public static WebApplication UseMvcConfiguration(this WebApplication app)
        {
            // Adicionando o HSTS a aplicação
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/erro/500");
                app.UseStatusCodePagesWithRedirects("/erro/{0}");
                app.UseHsts();
            }

            app.UseGlobalizationConfig();

            app.UseElmahIo();
            app.UseElmahIoExtensionsLogging();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // Exemplo usando transformador de rota
            //app.MapControllerRoute(
            //    name: "default",
            //    pattern: "{controller:slugify=Home}/{action:slugify=Index}/{id?}");

            // Rotas para areas
            //app.MapControllerRoute(
            //    name: "areas",
            //    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            // Rota para áreas especializadas
            app.MapAreaControllerRoute("AreaProdutos", "Produtos", "Produtos/{controller=Cadastro}/{action=Index}/{id?}");
            app.MapAreaControllerRoute("AreaVendas", "Vendas", "Vendas/{controller=Gestao}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            // Maneira de fazer injeção de depend�ncia antes do start da aplicação
            using (var ServiceScope = app.Services.CreateScope())
            {
                var services = ServiceScope.ServiceProvider;

                var singService = services.GetRequiredService<IOperacaoSingleton>();

                Console.WriteLine("Direto da Program.cs" + singService.OperacaoId);
            }

            return app;
        }
    }
}
