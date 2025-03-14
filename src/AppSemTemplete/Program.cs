using AppSemTemplate.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddGlobalizationConfig()
    .AddElmahConfiguration()
    .AddMvcConfiguration()
    .AddIdentityConfiguration()
    .AddDependencyInjectionConfiguration();

var app = builder.Build();

app.UseMvcConfiguration();

app.Run();
