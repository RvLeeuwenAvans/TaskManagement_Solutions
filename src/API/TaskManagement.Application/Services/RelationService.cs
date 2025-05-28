using AutoMapper;
using FluentValidation;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Office.Relation;
using TaskManagement.DTO.Office.Relation;

namespace TaskManagement.Application.Services;

public class RelationService(
    IRelationRepository relationRepository,
    IMapper mapper,
    IValidator<CreateRelation> createValidator,
    IValidator<UpdateRelation> updateValidator)
{
    public Task<List<RelationResponse>> GetRelationsByOffice(Guid officeId)
    {
        var relations = relationRepository.GetAll()
            .Where(r => r.OfficeId == officeId)
            .ToList();

        var response = mapper.Map<List<RelationResponse>>(relations);

        return Task.FromResult(response);
    }

    public async Task<RelationResponse?> GetRelationByIdAsync(Guid id)
    {
        var relation = await relationRepository.GetByIdAsync(id);
        return relation == null ? null : mapper.Map<RelationResponse>(relation);
    }

    public async Task<RelationResponse> CreateRelationAsync(CreateRelation dto)
    {
        var validation = await createValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        var relation = mapper.Map<Relation>(dto);
        await relationRepository.AddAsync(relation);
        return mapper.Map<RelationResponse>(relation);
    }

    public async Task<bool> UpdateRelationAsync(UpdateRelation dto)
    {
        var validation = await updateValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        var relation = await relationRepository.GetByIdAsync(dto.Id);
        if (relation == null) return false;

        mapper.Map(dto, relation);
        await relationRepository.UpdateAsync(relation);
        return true;
    }

    public async Task<bool> DeleteRelationAsync(Guid id)
    {
        var relation = await relationRepository.GetByIdAsync(id);
        if (relation == null) return false;

        await relationRepository.DeleteAsync(id);
        return true;
    }
}