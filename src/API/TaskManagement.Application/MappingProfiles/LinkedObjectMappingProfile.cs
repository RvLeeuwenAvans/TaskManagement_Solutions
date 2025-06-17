using AutoMapper;
using TaskManagement.Domain.Office.User.Task.LinkedObject;
using TaskManagement.DTO.Office.User.Task.LinkedObject;

namespace TaskManagement.Application.MappingProfiles;

public class LinkedObjectMappingProfile : Profile
{
    public LinkedObjectMappingProfile()
    {
        CreateMap<CreateLinkedObject, LinkedObject>()
            .ForMember(dest => dest.TaskId, opt =>
                opt.MapFrom(src => src.UserTaskId))
            .ForMember(dest => dest.Relation,
                opt => opt.Ignore())
            .ForMember(dest => dest.DamageClaim,
                opt => opt.Ignore())
            .ForMember(dest => dest.InsurancePolicy,
                opt => opt.Ignore());

        CreateMap<UpdateLinkedObject, LinkedObject>()
            .ForMember(dest => dest.Relation,
                opt => opt.Ignore())
            .ForMember(dest => dest.DamageClaim,
                opt => opt.Ignore())
            .ForMember(dest => dest.InsurancePolicy,
                opt => opt.Ignore())
            .ForMember(dest => dest.UserTask,
                opt => opt.Ignore());

        CreateMap<LinkedObject, LinkedObjectResponse>()
            .ForMember(dest => dest.UserTaskId,
                opt =>
                    opt.MapFrom(src => src.UserTask.Id))
            .ForMember(dest => dest.RelationId,
                opt =>
                    opt.MapFrom(src => src.Relation != null ? src.Relation.Id : (Guid?)null))
            .ForMember(dest => dest.DamageClaimId,
                opt =>
                    opt.MapFrom(src => src.DamageClaim != null ? src.DamageClaim.Id : (Guid?)null))
            .ForMember(dest => dest.InsurancePolicyId,
                opt =>
                    opt.MapFrom(src => src.InsurancePolicy != null ? src.InsurancePolicy.Id : (Guid?)null));
    }
}