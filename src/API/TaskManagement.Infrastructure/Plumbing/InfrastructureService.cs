using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Office.User;
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

    public static IServiceCollection AddDatabaseSeeders(this IServiceCollection services)
    {
        services.AddScoped<ISeeder, OfficeSeeder>();
        services.AddScoped<ISeeder, UserSeeder>();
        services.AddScoped<ISeeder, RelationSeeder>();
        services.AddScoped<ISeeder, DamageClaimSeeder>();
        services.AddScoped<ISeeder, InsurancePolicySeeder>();
        services.AddScoped<ISeeder, UserTaskSeeder>();
        services.AddScoped<ISeeder, NoteSeeder>();
        services.AddScoped<ISeeder, LinkedObjectSeeder>();

        services.AddScoped<DatabaseSeederService>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        return services;
    }
}