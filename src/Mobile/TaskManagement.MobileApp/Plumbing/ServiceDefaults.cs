using TaskManagement.Client;
using TaskManagement.Client.Plumbing;
using Microsoft.Extensions.Configuration;
using TaskManagement.MobileApp.Models;
using TaskManagement.MobileApp.Models.Interfaces;
using TaskManagement.MobileApp.Properties;
using TaskManagement.MobileApp.Repositories;
using TaskManagement.MobileApp.Repositories.Interfaces;
using TaskManagement.MobileApp.Services;
using TaskManagement.MobileApp.Services.Authentication;

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
        services.AddSingleton<ITaskRepository, TaskRepository>();
        services.AddSingleton<TaskService>();
        
        return services;
    }
}