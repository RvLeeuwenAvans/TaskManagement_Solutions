using Microsoft.AspNetCore.Identity;
using TaskManagement.Domain.Office.User;

namespace TaskManagement.Infrastructure.Persistence.Seeders;

public static class UserSeeder
{
    public static async Task SeedAsync(TaskManagementDatabaseContext context)
    {
        if (!context.Users.Any())
        {
            var office = context.Offices.First();
            var passwordHasher = new PasswordHasher<User>();
            
            var users = new List<User>
            {
                new()
                {
                    FirstName = "John", 
                    LastName = "Doe",
                    Email = "john.doe@example.com", 
                    Password = "hashedpassword12",
                    OfficeId = office.Id,
                    Office = office
                },
                new()
                {
                    FirstName = "Jane", 
                    LastName = "Smith", 
                    Email = "jane.smith@example.com",
                        Password = "hashedpassword23", 
                    OfficeId = office.Id,
                    Office = office
                }
            };

            foreach (var user in users)
            {
                user.Password = passwordHasher.HashPassword(user, user.Password);
            }

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }
    }
}