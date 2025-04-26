using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Office.User;
using TaskManagement.DTO.Office.User;

namespace TaskManagement.Application.Services;

public class UserService(
    IUserRepository userRepository,
    IMapper mapper,
    IValidator<UserCreateDto> createValidator,
    IValidator<UserUpdateDto> updateValidator,
    IPasswordHasher<User> passwordHasher)
{
    public Task<List<UserResponseDto>> GetUsersFromOffice(Guid officeId)
    {
        var users = userRepository
            .GetAll()
            .Where(u => u.OfficeId == officeId)
            .ToList();

        var response = mapper.Map<List<UserResponseDto>>(users);

        return Task.FromResult(response);
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
        user.Password = HashPassword(user, dto.Password);

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

        if (dto.Password != null)
            user.Password = HashPassword(user, dto.Password);

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

    public bool VerifyPassword(User user, string hashedPassword, string enteredPassword)
    {
        var result = passwordHasher.VerifyHashedPassword(user, hashedPassword, enteredPassword);
        return result == PasswordVerificationResult.Success;
    }
    
    private string HashPassword(User user, string password)
    {
        return passwordHasher.HashPassword(user, password);
    }
}