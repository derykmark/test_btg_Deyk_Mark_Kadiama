using BankKRT.API.Middlewares;
using BankKRT.API.Filters;
using BankKRT.Application;
using BankKRT.Infrastructure;
using Microsoft.OpenApi.Models;
using FluentValidation.AspNetCore;
using Serilog;
using Serilog.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog early so it captures startup logs
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentName()
    .Enrich.WithProcessId()
    .Enrich.WithThreadId()
    .Enrich.WithExceptionDetails()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});

builder.Services.AddFluentValidationAutoValidation();

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
