using TaskManagement.Infrastructure.Plumbing;

namespace TaskManagement.API.Plumbing;

public static class ServiceDefaults
{
    public static IServiceCollection ConfigureDefaultServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.ConfigureDefaults(configuration);

        return services;
    }
}