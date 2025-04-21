using AutoMapper;
using TaskManagement.Domain.Office.User.Task;
using TaskManagement.DTO.Office.User.Task;

namespace TaskManagement.Application.MappingProfiles;

public class UserTaskMappingProfile : Profile
{
    public UserTaskMappingProfile()
    {
        CreateMap<UserTask, UserTaskResponseDto>();
        CreateMap<UserTaskCreateDto, UserTask>();
        CreateMap<UserTaskUpdateDto, UserTask>()
            .ForAllMembers(opts =>
                opts.Condition((src, dest, srcMember) => srcMember is not null));
    }
}