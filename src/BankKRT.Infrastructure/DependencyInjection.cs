using BankKRT.Domain.Interfaces;
using BankKRT.Infrastructure.Data;
using BankKRT.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BankKRT.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Setup DbContext with PostgreSQL
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        // Add Repository
        services.AddScoped<IAccountRepository, AccountRepository>();

        // Add Memory Cache
        services.AddMemoryCache();

        // Add MediatR (Handlers inside this assembly)
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }
}
