using AutoMapper;
using FluentValidation;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Office.User.Task;
using TaskManagement.DTO.Office.User.Task;

namespace TaskManagement.Application.Services;

public class UserTaskService(
    IUserTaskRepository taskRepository,
    IMapper mapper,
    IValidator<CreateUserTask> createValidator,
    IValidator<UpdateUserTask> updateValidator)
{
    public Task<List<UserTaskResponse>> GetTasksByUser(Guid userId)
    {
        var tasks = taskRepository.GetAll()
            .Where(t => t.UserId == userId)
            .ToList();
        
        var response = mapper.Map<List<UserTaskResponse>>(tasks);

        return Task.FromResult(response);
    }

    public async Task<UserTaskResponse?> GetTaskByIdAsync(Guid id)
    {
        var task = await taskRepository.GetByIdAsync(id);
        return task is null ? null : mapper.Map<UserTaskResponse>(task);
    }

    public async Task<UserTaskResponse> CreateTaskAsync(CreateUserTask dto)
    {
        var validationResult = await createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var task = mapper.Map<UserTask>(dto);
        await taskRepository.AddAsync(task);
        return mapper.Map<UserTaskResponse>(task);
    }

    public async Task<bool> UpdateTaskAsync(UpdateUserTask dto)
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