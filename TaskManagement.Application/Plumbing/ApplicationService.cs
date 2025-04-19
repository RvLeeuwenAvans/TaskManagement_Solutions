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
        services.AddAutoMapper(typeof(UserTaskMappingProfile));
        services.AddAutoMapper(typeof(NoteMappingProfile));
        services.AddAutoMapper(typeof(RelationMappingProfile));
        
        services.AddScoped<OfficeService>();
        services.AddScoped<UserService>();
        services.AddScoped<UserTaskService>();
        services.AddScoped<NoteService>();
        services.AddScoped<RelationService>();
        
        return services;
    }
}