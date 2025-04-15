using Microsoft.EntityFrameworkCore;
using TaskManagement.Appplication.Interfaces;
using TaskManagement.Domain;
using TaskManagement.Domain.Office;
using TaskManagement.Domain.Office.Relation;
using TaskManagement.Domain.Office.Relation.DamageClaim;
using TaskManagement.Domain.Office.Relation.InsurancePolicy;
using TaskManagement.Domain.Office.User;
using TaskManagement.Domain.Office.User.Task;
using TaskManagement.Domain.Office.User.Task.Note;


namespace TaskManagement.Infrastructure.Persistence;

public class TaskManagementDatabaseContext(DbContextOptions<TaskManagementDatabaseContext> options)
    : DbContext(options), IDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserTask> Tasks { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<Office> Offices { get; set; }
    public DbSet<Relation> Relations { get; set; }
    public DbSet<DamageClaim> DamageClaims { get; set; }
    public DbSet<LinkedObject> LinkedObjects { get; set; }
    public DbSet<InsurancePolicy> InsurancePolicies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply Fluent API configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskManagementDatabaseContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Optional: Add logic for auditing, etc., before saving changes
        ChangeTracker.DetectChanges();

        return await base.SaveChangesAsync(cancellationToken);
    }
}