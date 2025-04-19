using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Office.User;
using TaskManagement.DTO.Office.User;

namespace TaskManagement.Application.Services;

public class UserService(
    IUserRepository userRepository,
    IMapper mapper,
    IValidator<UserCreateDto> createValidator,
    IValidator<UserUpdateDto> updateValidator
)
{
    public async Task<List<UserResponseDto>> GetUsersFromOffice(Guid officeId)
    {
        var users = await userRepository
            .GetAll()
            .Where(u => u.OfficeId == officeId)
            .ToListAsync();

        return mapper.Map<List<UserResponseDto>>(users);
    }

    public async Task<UserResponseDto?> GetUserByIdAsync(Guid id)
    {
        var user = await userRepository.GetByIdAsync(id);
        return user is null ? null : mapper.Map<UserResponseDto>(user);
    }

    public async Task<UserResponseDto> CreateUserAsync(UserCreateDto dto)
    {
        var validationResult = await createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var user = mapper.Map<User>(dto);
        await userRepository.AddAsync(user);
        return mapper.Map<UserResponseDto>(user);
    }

    public async Task<bool> UpdateUserAsync(UserUpdateDto dto)
    {
        var validationResult = await updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var user = await userRepository.GetByIdAsync(dto.Id);
        if (user is null)
            return false;

        mapper.Map(dto, user);
        await userRepository.UpdateAsync(user);
        return true;
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user is null)
            return false;

        await userRepository.DeleteAsync(id);
        return true;
    }
}