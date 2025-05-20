using TaskManagement.Client;
using TaskManagement.Client.Plumbing;
using Microsoft.Extensions.Configuration;
using TaskManagement.MobileApp.Properties;

namespace TaskManagement.MobileApp.Plumbing;

public static class ServiceDefaults
{
    public static IServiceCollection ConfigureDefaultServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = configuration.GetRequiredSection("ApiSettings").Get<ApiSettings>();
        
        services.Configure<ApiClientConfig>(config => { config.BaseUrl = settings!.BaseUrl; });
        services.RegisterClients();

        return services;
    }
}