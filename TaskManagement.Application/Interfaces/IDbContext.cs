using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Office;
using TaskManagement.Domain.Office.Relation;
using TaskManagement.Domain.Office.Relation.DamageClaim;
using TaskManagement.Domain.Office.Relation.InsurancePolicy;
using TaskManagement.Domain.Office.User;
using TaskManagement.Domain.Office.User.Task;
using TaskManagement.Domain.Office.User.Task.LinkedObject;
using TaskManagement.Domain.Office.User.Task.Note;

namespace TaskManagement.Application.Interfaces;

public interface IDbContext
{
    public DbSet<User> Users { get; }
    public DbSet<UserTask> Tasks { get; }
    public DbSet<Note> Notes { get; }
    public DbSet<Office> Offices { get; }
    public DbSet<Relation> Relations { get; }
    public DbSet<DamageClaim> DamageClaims { get; }
    public DbSet<LinkedObject> LinkedObjects { get; }
    public DbSet<InsurancePolicy> InsurancePolicies { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}