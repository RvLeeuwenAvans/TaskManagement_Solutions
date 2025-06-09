using TaskManagement.Client;
using TaskManagement.Client.Plumbing;
using Microsoft.Extensions.Configuration;
using TaskManagement.MobileApp.Models;
using TaskManagement.MobileApp.Models.Interfaces;
using TaskManagement.MobileApp.Properties;
using TaskManagement.MobileApp.Services;
using TaskManagement.MobileApp.Services.Authentication;
using TaskManagement.MobileApp.Services.Repositories;
using TaskManagement.MobileApp.Services.Repositories.Interfaces;

namespace TaskManagement.MobileApp.Plumbing;

public static class ServiceDefaults
{
    public static IServiceCollection ConfigureDefaultServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = configuration.GetRequiredSection("ApiSettings").Get<ApiSettings>();
        services.AddSingleton(new ApiClientConfig { BaseUrl = settings!.BaseUrl });
        services.RegisterClients();

        // Auth
        services.AddSingleton<IUserContext, UserContext>();
        services.AddSingleton<IAuthRepository, AuthRepository>();
        services.AddSingleton<AuthService>();
        // Services
        services.AddSingleton<IDamageClaimRepository, DamageClaimRepository>();
        services.AddSingleton<IPolicyRepository, InsurancePolicyRepository>();
        services.AddSingleton<IRelationRepository, RelationRepository>();
        services.AddSingleton<LinkedObjectService>();
        
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<OfficeService>();

        services.AddSingleton<ITaskRepository, TaskRepository>();
        services.AddSingleton<TaskService>();

        services.AddSingleton<ILinkedObjectRepository, LinkedObjectRepository>();
        services.AddSingleton<LinkedObjectService>();
        
        return services;
    }
}