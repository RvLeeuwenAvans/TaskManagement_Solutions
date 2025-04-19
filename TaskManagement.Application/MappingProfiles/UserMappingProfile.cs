using AutoMapper;
using TaskManagement.Domain.Office.User;
using TaskManagement.DTO.Office.User;

namespace TaskManagement.Application.MappingProfiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserResponseDto>();
        CreateMap<UserCreateDto, User>();
        CreateMap<UserUpdateDto, User>()
            .ForAllMembers(
                opts => opts.Condition(
                    (src, dest, srcMember) => srcMember != null
                )
            );
    }
}