using AutoMapper;
using TaskManagement.Domain.Office;
using TaskManagement.DTO.Office;

namespace TaskManagement.Application.MappingProfiles;

public class OfficeMappingProfile : Profile
{
    public OfficeMappingProfile()
    {
        CreateMap<Office, OfficeResponse>();
        CreateMap<CreateOffice, Office>();
        CreateMap<UpdateOffice, Office>()
            .ForMember(dest => dest.Name, opt =>
                opt.Condition(src => !string.IsNullOrWhiteSpace(src.Name)));
    }
}