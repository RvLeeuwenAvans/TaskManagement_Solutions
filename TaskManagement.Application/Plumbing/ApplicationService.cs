using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Application.MappingProfiles;

namespace TaskManagement.Application.Plumbing;

public static class ApplicationService
{
    public static IServiceCollection ConfigureDefaults(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(UserMappingProfile));

        return services;
    }
}