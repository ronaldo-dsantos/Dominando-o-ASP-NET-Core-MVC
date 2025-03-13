using AppSemTemplate.Data;
using Microsoft.AspNetCore.Identity;

namespace AppSemTemplate.Configuration
{
    public static class IdentityConfig
    {
        public static WebApplicationBuilder AddIdentityConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();

            builder.Services.AddAuthorizationBuilder()
                .AddPolicy("PodeExcluirPermanentemente", policy =>
                    policy.RequireRole("Admin"))
                .AddPolicy("VerProdutos", policy =>
                    policy.RequireClaim("Produtos", "VI"));

            return builder;
        }
    }
}
