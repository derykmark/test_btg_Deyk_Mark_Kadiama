using BankKRT.Application.Interfaces;
using BankKRT.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BankKRT.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        
        var assembly = typeof(DependencyInjection).Assembly;
        
        services.AddValidatorsFromAssembly(assembly);
        
        services.AddMediatR(config => 
        {
            config.RegisterServicesFromAssembly(assembly);
        });

        return services;
    }
}
