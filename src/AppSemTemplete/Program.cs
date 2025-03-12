using AppSemTemplate.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddElmahConfiguration()
    .AddMvcConfiguration()
    .AddIdentityConfiguration()
    .AddDependencyInjectionConfiguration();

var app = builder.Build();

app.UseMvcConfiguration();

app.Run();
