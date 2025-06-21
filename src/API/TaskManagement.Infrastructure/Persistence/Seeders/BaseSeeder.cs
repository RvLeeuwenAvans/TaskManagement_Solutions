using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TaskManagement.Infrastructure.Persistence.Seeders;

public abstract class BaseSeeder(TaskManagementDatabaseContext context, ILogger logger) : ISeeder
{
    protected readonly TaskManagementDatabaseContext Context = context;
    protected readonly ILogger Logger = logger;

    public abstract Task SeedAsync();
    public abstract int Order { get; }
    public abstract string Name { get; }

    protected async Task<bool> HasDataAsync<T>() where T : class
    {
        return await Context.Set<T>().AnyAsync();
    }
}