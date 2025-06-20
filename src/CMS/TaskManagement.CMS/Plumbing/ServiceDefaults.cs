using TaskManagement.Client;
using TaskManagement.Client.Plumbing;
using TaskManagement.CMS.Properties;
using TaskManagement.CMS.Services;
using TaskManagement.CMS.Services.Interfaces;

namespace TaskManagement.CMS.Plumbing;

public static class ServiceDefaults
{
    public static IServiceCollection ConfigureDefaultServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = configuration.GetRequiredSection("ApiSettings").Get<ApiSettings>();
        services.AddSingleton(new ApiClientConfig { BaseUrl = settings!.BaseUrl });
        services.RegisterClients();
        
        services.AddScoped<IOfficeService, OfficeService>();
        services.AddScoped<IUserService, UserService>();
        
        return services;
    }
}