using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Infrastructure.Persistence;

namespace TaskManagement.Infrastructure.Plumbing;

public static class InfrastructureService
{
    public static IServiceCollection ConfigureDefaults(this IServiceCollection services, IConfiguration configuration)
    {
        // Add DB context with MariaDB provider
        services.AddDbContext<TaskManagementDatabaseContext>(options =>
            options.UseMySql(
                configuration.GetConnectionString("DefaultConnection"),
                ServerVersion.AutoDetect(
                    configuration.GetConnectionString("DefaultConnection"))
            ).EnableSensitiveDataLogging().LogTo(Console.WriteLine)
        );
        
        return services;
    }
}