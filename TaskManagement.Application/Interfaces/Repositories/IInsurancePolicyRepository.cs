using TaskManagement.Domain.Office.Relation.InsurancePolicy;

namespace TaskManagement.Application.Interfaces.Repositories;

public interface IInsurancePolicyRepository
{
    IQueryable<InsurancePolicy> GetAll();
    Task<InsurancePolicy?> GetByIdAsync(Guid id);
    Task AddAsync(InsurancePolicy insurancePolicy);
    Task UpdateAsync(InsurancePolicy insurancePolicy);
    Task DeleteAsync(Guid id);
}