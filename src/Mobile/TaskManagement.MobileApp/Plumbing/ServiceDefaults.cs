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
using TaskManagement.MobileApp.ViewModels.Modals;
using TaskManagement.MobileApp.Views.Modals;

namespace TaskManagement.MobileApp.Plumbing;

public static class ServiceDefaults
{
    public static IServiceCollection ConfigureDefaultServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<AuthenticatedEndpointExecutor>();
        
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
        
        services.AddSingleton<IOfficeRepository, OfficeRepository>();
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<OfficeService>();

        services.AddSingleton<ITaskRepository, TaskRepository>();
        services.AddSingleton<TaskService>();

        services.AddSingleton<ILinkedObjectRepository, LinkedObjectRepository>();
        services.AddSingleton<LinkedObjectService>();

        // Popups; using ContentPages as Model; because the toolkit version is way too unstable on android.
        services.AddTransient<TaskTypeFilterModalViewModel>();
        services.AddTransient<TaskTypeFilterModal>();
        
        return services;
    }
}