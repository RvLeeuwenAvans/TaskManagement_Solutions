using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Office;
using TaskManagement.DTO.Office;

namespace TaskManagement.Application.Services;

public class OfficeService(
    IOfficeRepository officeRepository,
    IMapper mapper,
    IValidator<OfficeCreateDto> createValidator,
    IValidator<OfficeUpdateDto> updateValidator)
{
    public async Task<List<OfficeResponseDto>> GetAllOfficesAsync()
    {
        var offices = await officeRepository.GetAll().ToListAsync();
        return mapper.Map<List<OfficeResponseDto>>(offices);
    }

    public async Task<OfficeResponseDto?> GetOfficeByIdAsync(Guid id)
    {
        var office = await officeRepository.GetByIdAsync(id);
        return office is null ? null : mapper.Map<OfficeResponseDto>(office);
    }

    public async Task<OfficeResponseDto> CreateOfficeAsync(OfficeCreateDto dto)
    {
        var validationResult = await createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var office = mapper.Map<Office>(dto);
        await officeRepository.AddAsync(office);
        return mapper.Map<OfficeResponseDto>(office);
    }

    public async Task<bool> UpdateOfficeAsync(OfficeUpdateDto dto)
    {
        var validationResult = await updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var office = await officeRepository.GetByIdAsync(dto.Id);
        if (office is null)
            return false;

        mapper.Map(dto, office);
        await officeRepository.UpdateAsync(office);
        return true;
    }

    public async Task<bool> DeleteOfficeAsync(Guid id)
    {
        var office = await officeRepository.GetByIdAsync(id);
        if (office is null)
            return false;

        await officeRepository.DeleteAsync(id);
        return true;
    }
}