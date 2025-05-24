using TaskManagement.Domain.Office.User.Task.Note;

namespace TaskManagement.Infrastructure.Persistence.Seeders;

public static class NoteSeeder
{
    public static async Task SeedAsync(TaskManagementDatabaseContext context)
    {
        if (!context.Notes.Any())
        {
            var task = context.Tasks.First();
            var notes = new List<Note>
            {
                new() { Content = "Note: Fix stuff", TaskId = task.Id, UserTask = task },
                new() { Content = "Note: More stuff", TaskId = task.Id, UserTask = task }
            };

            await context.Notes.AddRangeAsync(notes);
            await context.SaveChangesAsync();
        }
    }
}