using MudBlazor;
using TaskManagement.Client;
using TaskManagement.Client.Plumbing;
using TaskManagement.CMS.Services;
using TaskManagement.CMS.Services.Authentication;
using TaskManagement.MobileApp.Properties;

namespace TaskManagement.CMS.Plumbing;

public static class ServiceDefaults
{
    public static IServiceCollection ConfigureDefaultServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = configuration.GetRequiredSection("ApiSettings").Get<ApiSettings>();
        services.AddSingleton(new ApiClientConfig { BaseUrl = settings!.BaseUrl });
        services.RegisterClients();

        // Singleton to maintain state across the application
        services.AddScoped<AuthenticationService>();
        
        services.AddScoped<OfficeService>();
        services.AddScoped<UserService>();
        services.AddScoped<RelationService>();
        services.AddScoped<DamageClaimService>();
        services.AddScoped<InsurancePolicyService>();
        
        services.AddScoped<IDialogService, DialogService>();

        return services;
    }
}