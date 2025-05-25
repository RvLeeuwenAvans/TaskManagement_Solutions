using AutoMapper;
using TaskManagement.Domain.Office.User;
using TaskManagement.DTO.Office.User;

namespace TaskManagement.Application.MappingProfiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserResponse>();
        CreateMap<CreateUser, User>();
        CreateMap<UpdateUser, User>()
            .ForAllMembers(
                opts => 
                    opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}