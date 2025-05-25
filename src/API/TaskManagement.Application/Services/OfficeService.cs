using AutoMapper;
using FluentValidation;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Office;
using TaskManagement.DTO.Office;

namespace TaskManagement.Application.Services;

public class OfficeService(
    IOfficeRepository officeRepository,
    IMapper mapper,
    IValidator<CreateOffice> createValidator,
    IValidator<UpdateOffice> updateValidator)
{
    public Task<List<OfficeResponse>> GetAllOffices()
    {
        var offices = officeRepository.GetAll().ToList();
        var response = mapper.Map<List<OfficeResponse>>(offices);
        
        return Task.FromResult(response);
    }

    public async Task<OfficeResponse?> GetOfficeByIdAsync(Guid id)
    {
        var office = await officeRepository.GetByIdAsync(id);
        return office is null ? null : mapper.Map<OfficeResponse>(office);
    }

    public async Task<OfficeResponse> CreateOfficeAsync(CreateOffice dto)
    {
        var validationResult = await createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var office = mapper.Map<Office>(dto);
        await officeRepository.AddAsync(office);
        return mapper.Map<OfficeResponse>(office);
    }

    public async Task<bool> UpdateOfficeAsync(UpdateOffice dto)
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
