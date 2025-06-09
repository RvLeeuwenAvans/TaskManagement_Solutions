using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Office;
using TaskManagement.Domain.Office.Relation;
using TaskManagement.Domain.Office.Relation.DamageClaim;
using TaskManagement.Domain.Office.Relation.InsurancePolicy;
using TaskManagement.Domain.Office.User;
using TaskManagement.Domain.Office.User.Task;
using TaskManagement.Domain.Office.User.Task.LinkedObject;
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

        modelBuilder.Entity<User>()
            .HasOne(u => u.Office)
            .WithMany(o => o.Users)
            .HasForeignKey(u => u.OfficeId)
            .OnDelete(DeleteBehavior.Cascade);
        
        
        modelBuilder.Entity<Relation>()
            .HasOne(r => r.Office)
            .WithMany(o => o.Relations)
            .HasForeignKey(r => r.OfficeId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<UserTask>()
            .HasOne(ut => ut.LinkedObject)
            .WithOne(lo => lo.UserTask)
            .HasForeignKey<UserTask>(ut => ut.LinkedObjectId)
            .OnDelete(DeleteBehavior.SetNull); // When LinkedObject is deleted, set null

        modelBuilder.Entity<LinkedObject>()
            .HasOne(lo => lo.UserTask)
            .WithOne(ut => ut.LinkedObject)
            .HasForeignKey<LinkedObject>(lo => lo.TaskId)
            .OnDelete(DeleteBehavior.Cascade); // When UserTask is deleted, delete LinkedObject
        
        modelBuilder.Entity<LinkedObject>()
            .HasOne(lo => lo.Relation)
            .WithMany()
            .HasForeignKey(lo => lo.RelationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<LinkedObject>()
            .HasOne(lo => lo.InsurancePolicy)
            .WithMany()
            .HasForeignKey(lo => lo.InsurancePolicyId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<LinkedObject>()
            .HasOne(lo => lo.DamageClaim)
            .WithMany()
            .HasForeignKey(lo => lo.DamageClaimId)
            .OnDelete(DeleteBehavior.Cascade);
        
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