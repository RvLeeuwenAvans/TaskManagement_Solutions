namespace TaskManagement.Infrastructure.Persistence.Seeders;

public interface ISeeder
{
    Task SeedAsync();
    int Order { get; }
    string Name { get; }
}