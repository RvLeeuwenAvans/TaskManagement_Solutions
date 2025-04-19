using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Office.User.Task;
using TaskManagement.DTO.Office.User.Task;

namespace TaskManagement.Application.Services;

public class UserTaskService(
    IUserTaskRepository taskRepository,
    IMapper mapper,
    IValidator<UserTaskCreateDto> createValidator,
    IValidator<UserTaskUpdateDto> updateValidator)
{
    public async Task<List<UserTaskResponseDto>> GetAllTasksAsync()
    {
        var tasks = await taskRepository.GetAll().ToListAsync();
        return mapper.Map<List<UserTaskResponseDto>>(tasks);
    }

    public async Task<UserTaskResponseDto?> GetTaskByIdAsync(Guid id)
    {
        var task = await taskRepository.GetByIdAsync(id);
        return task is null ? null : mapper.Map<UserTaskResponseDto>(task);
    }

    public async Task<UserTaskResponseDto> CreateTaskAsync(UserTaskCreateDto dto)
    {
        var validationResult = await createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var task = mapper.Map<UserTask>(dto);
        await taskRepository.AddAsync(task);
        return mapper.Map<UserTaskResponseDto>(task);
    }

    public async Task<bool> UpdateTaskAsync(UserTaskUpdateDto dto)
    {
        var validationResult = await updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var task = await taskRepository.GetByIdAsync(dto.Id);
        if (task is null)
            return false;

        mapper.Map(dto, task);
        await taskRepository.UpdateAsync(task);
        return true;
    }

    public async Task<bool> DeleteTaskAsync(Guid id)
    {
        var task = await taskRepository.GetByIdAsync(id);
        if (task is null)
            return false;

        await taskRepository.DeleteAsync(id);
        return true;
    }
}