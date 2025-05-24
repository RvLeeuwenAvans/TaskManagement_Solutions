using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Infrastructure.Persistence;
using TaskManagement.Infrastructure.Persistence.Repositories;
using TaskManagement.Infrastructure.Persistence.Seeders;

namespace TaskManagement.Infrastructure.Plumbing;

public static class InfrastructureService
{
    public static IServiceCollection ConfigureInfraDefaults(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add DB context with MariaDB provider
        services.AddDbContext<TaskManagementDatabaseContext>(options =>
            options.UseMySql(
                configuration.GetConnectionString("DefaultConnection"),
                ServerVersion.AutoDetect(
                    configuration.GetConnectionString("DefaultConnection"))
            ).EnableSensitiveDataLogging().LogTo(Console.WriteLine)
        );

        services.AddScoped<INoteRepository, NoteRepository>();
        services.AddScoped<IDbContext, TaskManagementDatabaseContext>();
        services.AddScoped<IOfficeRepository, OfficeRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserTaskRepository, UserTaskRepository>();
        services.AddScoped<IRelationRepository, RelationRepository>();
        services.AddScoped<IDamageClaimRepository, DamageClaimRepository>();
        services.AddScoped<IInsurancePolicyRepository, InsurancePolicyRepository>();
        services.AddScoped<ILinkedObjectRepository, LinkedObjectRepository>();

        return services;
    }

    public static async void SeedDatabase(this IServiceCollection services)
    {
            var context = services.BuildServiceProvider().GetRequiredService<TaskManagementDatabaseContext>();
            await DatabaseSeeder.SeedAsync(context);
    }
}