using AutoMapper;
using TaskManagement.Domain.Office.User.Task;
using TaskManagement.DTO.Office.User;
using TaskManagement.DTO.Office.User.Task;

namespace TaskManagement.Application.MappingProfiles;

public class UserTaskMappingProfile : Profile
{
    public UserTaskMappingProfile()
    {
        CreateMap<UserTask, UserTaskResponseDto>()
            .ForMember(dest => dest.User,
                opt =>
                    opt.MapFrom(source => new UserResponseDto
                    {
                        Id = source.User.Id,
                        FirstName = source.User.FirstName,
                        LastName = source.User.LastName,
                        Email = source.User.Email,
                    }));
        CreateMap<UserTaskCreateDto, UserTask>();
        CreateMap<UserTaskUpdateDto, UserTask>()
            .ForAllMembers(opts =>
                opts.Condition((src, dest, srcMember) => srcMember is not null));
    }
}