using AutoMapper;
using TaskManagement.Domain.Office.Relation.InsurancePolicy;
using TaskManagement.DTO.Office.Relation.InsurancePolicy;

namespace TaskManagement.Application.MappingProfiles
{
    public class InsurancePolicyMappingProfile : Profile
    {
        public InsurancePolicyMappingProfile()
        {
            CreateMap<InsurancePolicy, InsurancePolicyResponse>();
            CreateMap<CreateInsurancePolicy, InsurancePolicy>();
            CreateMap<UpdateInsurancePolicy, InsurancePolicy>()
                .ForMember(dest => dest.Type, opt =>
                    opt.Condition(src => !string.IsNullOrWhiteSpace(src.Type)));
        }
    }
}