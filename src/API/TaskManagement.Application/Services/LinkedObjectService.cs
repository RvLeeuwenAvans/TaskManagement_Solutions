using AutoMapper;
using FluentValidation;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Office.User.Task.LinkedObject;
using TaskManagement.DTO.Office.User.Task.LinkedObject;

namespace TaskManagement.Application.Services
{
    public class LinkedObjectService(
        ILinkedObjectRepository linkedObjectRepository,
        IUserTaskRepository userTaskRepository,
        IMapper mapper,
        IValidator<CreateLinkedObject> createValidator)
    {
        public async Task<LinkedObjectResponse> CreateLinkedObjectAsync(CreateLinkedObject dto)
        {
            var validation = await createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);
            
            var linkedObject = mapper.Map<LinkedObject>(dto);
            await linkedObjectRepository.AddAsync(linkedObject);
            
            var userTask = await userTaskRepository.GetByIdAsync(dto.UserTaskId);
            
            // supress null warning for userTask this is a fatal exception; error should be thrown up the chain.
            userTask!.LinkedObject = linkedObject;
            await userTaskRepository.UpdateAsync(userTask);
            
            return mapper.Map<LinkedObjectResponse>(linkedObject);
        }
    }
}