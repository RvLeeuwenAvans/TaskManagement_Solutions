using TaskManagement.Application.Plumbing;
using TaskManagement.Infrastructure.Plumbing;

namespace TaskManagement.API.Plumbing;

public static class ServiceDefaults
{
    public static IServiceCollection ConfigureDefaultServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.ConfigureInfraDefaults(configuration);
        services.ConfigureAppDefaults();

        return services;
    }
}