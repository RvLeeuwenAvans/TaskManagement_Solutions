using AutoMapper;
using TaskManagement.Domain.Office.Relation.InsurancePolicy;
using TaskManagement.DTO.Office.Relation.InsurancePolicy;

namespace TaskManagement.Application.MappingProfiles
{
    public class InsurancePolicyMappingProfile : Profile
    {
        public InsurancePolicyMappingProfile()
        {
            CreateMap<InsurancePolicy, InsurancePolicyResponseDto>();
            CreateMap<InsurancePolicyCreateDto, InsurancePolicy>();
            CreateMap<InsurancePolicyUpdateDto, InsurancePolicy>()
                .ForMember(dest => dest.Type, opt =>
                    opt.Condition(src => !string.IsNullOrWhiteSpace(src.Type)));
        }
    }
}