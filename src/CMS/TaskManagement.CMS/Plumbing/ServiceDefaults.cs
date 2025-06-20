using MudBlazor;
using TaskManagement.Client;
using TaskManagement.Client.Plumbing;
using TaskManagement.CMS.Services;
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

        services.AddScoped<OfficeService>();
        services.AddScoped<UserService>();
        services.AddScoped<RelationService>();
        services.AddScoped<DamageClaimService>();
        services.AddScoped<InsurancePolicyService>();
        
        services.AddScoped<OfficeService>();
        services.AddScoped<IDialogService, DialogService>();

        return services;
    }
}