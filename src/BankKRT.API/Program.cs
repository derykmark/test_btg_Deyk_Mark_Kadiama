using BankKRT.API.Middlewares;
using BankKRT.API.Filters;
using BankKRT.Application;
using BankKRT.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BankKRT API",
        Version = "v1",
        Description = "API de Gerenciamento de Contas - Banco KRT"
    });
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();

// Necessário para WebApplicationFactory nos testes de integração
public partial class Program { }
