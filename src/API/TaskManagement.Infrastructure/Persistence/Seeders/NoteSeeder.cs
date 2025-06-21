using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagement.Domain.Office.User.Task.Note;

namespace TaskManagement.Infrastructure.Persistence.Seeders;

public class NoteSeeder(TaskManagementDatabaseContext context, ILogger<NoteSeeder> logger)
    : BaseSeeder(context, logger)
{
    public override int Order => 7;
    public override string Name => "Notes";

    public override async Task SeedAsync()
    {
        if (await HasDataAsync<Note>()) 
        {
            Logger.LogInformation("Notes already seeded, skipping...");
            return;
        }

        Logger.LogInformation("Seeding notes...");

        var task = await Context.Tasks.FirstAsync(t => t.Title == "Onderhoud doorvoeren");
        var notes = new List<Note>
        {
            new() { Content = "Contact opnemen met gebruiker", TaskId = task.Id, UserTask = task },
            new() { Content = "Documenten opvragen", TaskId = task.Id, UserTask = task }
        };

        await Context.Notes.AddRangeAsync(notes);
        await Context.SaveChangesAsync();
        
        Logger.LogInformation("Seeded {Count} notes", notes.Count);
    }
}