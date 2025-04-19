using AutoMapper;
using TaskManagement.Domain.Office.Relation.InsurancePolicy;
using TaskManagement.DTO.Office.Relation.InsurancePolicy;

namespace TaskManagement.Application.MappingProfiles
{
    public class InsurancePolicyMappingProfile : Profile
    {
        public InsurancePolicyMappingProfile()
        {
            // Map from InsurancePolicy to InsurancePolicyResponseDto
            CreateMap<InsurancePolicy, InsurancePolicyResponseDto>();

            // Map from InsurancePolicyCreateDto to InsurancePolicy
            CreateMap<InsurancePolicyCreateDto, InsurancePolicy>();

            // Map from InsurancePolicyUpdateDto to InsurancePolicy
            CreateMap<InsurancePolicyUpdateDto, InsurancePolicy>()
                .ForMember(dest => dest.Type, opt =>
                    opt.Condition(src => !string.IsNullOrWhiteSpace(src.Type)));
        }
    }
}