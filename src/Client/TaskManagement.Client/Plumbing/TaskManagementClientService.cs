using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Client.Clients;

namespace TaskManagement.Client.Plumbing;

public static class TaskManagementClientService
{
    public static IServiceCollection RegisterClients(this IServiceCollection services)
    {
        services.AddSingleton<HttpClient>();
        
        services.AddTransient<UserAuthenticationClient>();
        
        services.AddTransient<RelationClient>();
        services.AddTransient<OfficeClient>();
        services.AddTransient<UserClient>();
        services.AddTransient<UserTaskClient>();
        services.AddTransient<NoteClient>();
        services.AddTransient<InsurancePolicyClient>();
        services.AddTransient<DamageClaimClient>();
        services.AddTransient<LinkedObjectClient>();
        
        return services;
    }
}
