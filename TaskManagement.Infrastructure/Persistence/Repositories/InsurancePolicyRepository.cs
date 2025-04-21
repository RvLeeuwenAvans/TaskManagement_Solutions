using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Office.Relation.InsurancePolicy;

namespace TaskManagement.Infrastructure.Persistence.Repositories;

public class InsurancePolicyRepository(IDbContext context) : IInsurancePolicyRepository
{
    public IQueryable<InsurancePolicy> GetAll()
    {
        return context.InsurancePolicies.AsQueryable();
    }

    public async Task<InsurancePolicy?> GetByIdAsync(Guid id)
    {
        return await context.InsurancePolicies.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task AddAsync(InsurancePolicy insurancePolicy)
    {
        await context.InsurancePolicies.AddAsync(insurancePolicy);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(InsurancePolicy insurancePolicy)
    {
        context.InsurancePolicies.Update(insurancePolicy);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var insurancePolicy = await context.InsurancePolicies.FindAsync(id);
        if (insurancePolicy != null)
        {
            context.InsurancePolicies.Remove(insurancePolicy);
            await context.SaveChangesAsync();
        }
    }
}