using TaskManagement.DTO.Office.Relation.InsurancePolicy;

namespace TaskManagement.MobileApp.Services.Repositories.Interfaces;

public interface IPolicyRepository
{
    Task<InsurancePolicyResponse> GetInsurancePolicyAsync(Guid policyId);
}