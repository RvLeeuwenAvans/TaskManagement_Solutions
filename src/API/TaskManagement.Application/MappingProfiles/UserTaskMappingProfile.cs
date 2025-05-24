using AutoMapper;
using TaskManagement.Domain.Office.User.Task;
using TaskManagement.DTO.Office.User;
using TaskManagement.DTO.Office.User.Task;
using TaskManagement.DTO.Office.User.Task.LinkedObject;

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
                    }))
            .AfterMap((source, dest) =>
            {
                if (source.LinkedObject != null)
                {
                    dest.LinkedObject = new LinkedObjectResponseDto
                    {
                        Id = source.LinkedObject.Id,
                        UserTaskId = source.LinkedObject.UserTask?.Id ?? Guid.Empty,
                        RelationId = source.LinkedObject.Relation?.Id,
                        DamageClaimId = source.LinkedObject.DamageClaim?.Id,
                        InsurancePolicyId = source.LinkedObject.InsurancePolicy?.Id
                    };
                }
                else
                {
                    dest.LinkedObject = null;
                }
            });
        
        CreateMap<UserTaskCreateDto, UserTask>();
        CreateMap<UserTaskUpdateDto, UserTask>()
            .ForAllMembers(opts =>
                opts.Condition((src, dest, srcMember) => srcMember is not null));
    }
}