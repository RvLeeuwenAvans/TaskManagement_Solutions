using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Office.Relation.InsurancePolicy;
using TaskManagement.DTO.Office.Relation.InsurancePolicy;

namespace TaskManagement.Application.Services;

public class InsurancePolicyService(
    IInsurancePolicyRepository insurancePolicyRepository,
    IMapper mapper,
    IValidator<InsurancePolicyCreateDto> createValidator,
    IValidator<InsurancePolicyUpdateDto> updateValidator)
{
    public Task<List<InsurancePolicyResponseDto>> GetInsurancePoliciesByOffice(Guid officeId)
    {
        var insurancePolicies = insurancePolicyRepository.GetAll()
            .Where(ip => ip.Relation.OfficeId == officeId)
            .ToList();

        var response = mapper.Map<List<InsurancePolicyResponseDto>>(insurancePolicies);
        
        return Task.FromResult(response);
    }

    public async Task<InsurancePolicyResponseDto?> GetInsurancePolicyByIdAsync(Guid id)
    {
        var insurancePolicy = await insurancePolicyRepository.GetByIdAsync(id);
        return insurancePolicy == null ? null : mapper.Map<InsurancePolicyResponseDto>(insurancePolicy);
    }

    public async Task<InsurancePolicyResponseDto> CreateInsurancePolicyAsync(InsurancePolicyCreateDto dto)
    {
        var validation = await createValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        var insurancePolicy = mapper.Map<InsurancePolicy>(dto);
        await insurancePolicyRepository.AddAsync(insurancePolicy);
        return mapper.Map<InsurancePolicyResponseDto>(insurancePolicy);
    }

    public async Task<bool> UpdateInsurancePolicyAsync(InsurancePolicyUpdateDto dto)
    {
        var validation = await updateValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        var insurancePolicy = await insurancePolicyRepository.GetByIdAsync(dto.Id);
        if (insurancePolicy == null) return false;

        mapper.Map(dto, insurancePolicy);
        await insurancePolicyRepository.UpdateAsync(insurancePolicy);
        return true;
    }

    public async Task<bool> DeleteInsurancePolicyAsync(Guid id)
    {
        var insurancePolicy = await insurancePolicyRepository.GetByIdAsync(id);
        if (insurancePolicy == null) return false;

        await insurancePolicyRepository.DeleteAsync(id);
        return true;
    }
}