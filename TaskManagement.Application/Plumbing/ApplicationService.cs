using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Application.MappingProfiles;
using TaskManagement.Application.Services;

namespace TaskManagement.Application.Plumbing;

public static class ApplicationService
{
    public static IServiceCollection ConfigureAppDefaults(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(UserMappingProfile));
        services.AddAutoMapper(typeof(OfficeMappingProfile));

        services.AddScoped<OfficeService>();
        services.AddScoped<UserService>();
        
        return services;
    }
}