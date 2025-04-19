using AutoMapper;
using TaskManagement.Domain.Office.Relation;
using TaskManagement.DTO.Office.Relation;

namespace TaskManagement.Application.MappingProfiles;

public class RelationMappingProfile : Profile
{
    public RelationMappingProfile()
    {
        CreateMap<Relation, RelationResponseDto>();
        CreateMap<RelationCreateDto, Relation>();
        CreateMap<RelationUpdateDto, Relation>()
            .ForMember(dest => dest.FirstName, opt =>
                opt.Condition(src => !string.IsNullOrWhiteSpace(src.FirstName)))
            .ForMember(dest => dest.LastName, opt =>
                opt.Condition(src => !string.IsNullOrWhiteSpace(src.LastName)));
    }
}