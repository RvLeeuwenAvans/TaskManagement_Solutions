using AutoMapper;
using TaskManagement.Domain.Office.Relation.DamageClaim;
using TaskManagement.DTO.Office.Relation.DamageClaim;

namespace TaskManagement.Application.MappingProfiles
{
    public class DamageClaimMappingProfile : Profile
    {
        public DamageClaimMappingProfile()
        {
            // Map from DamageClaim to DamageClaimResponseDto
            CreateMap<DamageClaim, DamageClaimResponseDto>();

            // Map from DamageClaimCreateDto to DamageClaim
            CreateMap<DamageClaimCreateDto, DamageClaim>();

            // Map from DamageClaimUpdateDto to DamageClaim
            CreateMap<DamageClaimUpdateDto, DamageClaim>()
                .ForMember(dest => dest.Type, opt =>
                    opt.Condition(src => !string.IsNullOrWhiteSpace(src.Type)));
        }
    }
}