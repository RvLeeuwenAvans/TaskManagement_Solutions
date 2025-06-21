using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TaskManagement.Infrastructure.Persistence.Seeders;

public static class HostExtensions
{
    public static async Task SeedDatabaseAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var seederService = scope.ServiceProvider.GetRequiredService<DatabaseSeederService>();
        await seederService.SeedAllAsync();
    }
}