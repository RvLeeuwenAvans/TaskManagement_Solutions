using AutoMapper;
using TaskManagement.Domain.Office.Relation.DamageClaim;
using TaskManagement.DTO.Office.Relation.DamageClaim;

namespace TaskManagement.Application.MappingProfiles
{
    public class DamageClaimMappingProfile : Profile
    {
        public DamageClaimMappingProfile()
        {
            CreateMap<DamageClaim, DamageClaimResponse>();
            CreateMap<CreateDamageClaim, DamageClaim>();
            CreateMap<UpdateDamageClaim, DamageClaim>()
                .ForMember(dest => dest.Type, opt =>
                    opt.Condition(src => !string.IsNullOrWhiteSpace(src.Type)));
        }
    }
}