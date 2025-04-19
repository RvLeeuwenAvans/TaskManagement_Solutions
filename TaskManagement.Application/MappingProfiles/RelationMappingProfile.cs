using AutoMapper;
using TaskManagement.Domain.Office.Relation;
using TaskManagement.DTO.Office.Relation;

namespace TaskManagement.Application.MappingProfiles;

public class RelationMappingProfile : Profile
{
    public RelationMappingProfile()
    {
        // Map from Relation to RelationResponseDto
        CreateMap<Relation, RelationResponseDto>();

        // Map from RelationCreateDto to Relation
        CreateMap<RelationCreateDto, Relation>();

        // Map from RelationUpdateDto to Relation
        CreateMap<RelationUpdateDto, Relation>()
            .ForMember(dest => dest.FirstName, opt =>
                opt.Condition(src => !string.IsNullOrWhiteSpace(src.FirstName)))
            .ForMember(dest => dest.LastName, opt =>
                opt.Condition(src => !string.IsNullOrWhiteSpace(src.LastName)));
    }
}