using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TaskManagement.Application.Plumbing;
using TaskManagement.Infrastructure.Plumbing;

namespace TaskManagement.API.Plumbing;

public static class ServiceDefaults
{
    public static IServiceCollection ConfigureDefaultServices(this IServiceCollection services,
        IConfiguration configuration, bool development = true)
    {
        services = ConfigureAuthenticationDefaults(services, configuration);
        
        services.ConfigureInfraDefaults(configuration);
        services.ConfigureAppDefaults();
        
        // Comment when running migration for the first time; otherwise it'll fail; i should prob move this.
        if (development)
        {
            services.SeedDatabase();
        }

        return services;
    }

    private static IServiceCollection ConfigureAuthenticationDefaults(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorization();
        
        return services;
    }
}