using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Office.Relation;
using TaskManagement.DTO.Office.Relation;

namespace TaskManagement.Application.Services;

public class RelationService(
    IRelationRepository relationRepository,
    IMapper mapper,
    IValidator<RelationCreateDto> createValidator,
    IValidator<RelationUpdateDto> updateValidator)
{
    public async Task<List<RelationResponseDto>> GetAllRelationsAsync()
    {
        var relations = await relationRepository.GetAll().ToListAsync();
        return mapper.Map<List<RelationResponseDto>>(relations);
    }

    public async Task<RelationResponseDto?> GetRelationByIdAsync(Guid id)
    {
        var relation = await relationRepository.GetByIdAsync(id);
        return relation == null ? null : mapper.Map<RelationResponseDto>(relation);
    }

    public async Task<RelationResponseDto> CreateRelationAsync(RelationCreateDto dto)
    {
        var validation = await createValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        var relation = mapper.Map<Relation>(dto);
        await relationRepository.AddAsync(relation);
        return mapper.Map<RelationResponseDto>(relation);
    }

    public async Task<bool> UpdateRelationAsync(RelationUpdateDto dto)
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