using AutoMapper;
using TaskManagement.Domain.Office.User.Task.Note;
using TaskManagement.DTO.Office.User.Task.Note;

namespace TaskManagement.Application.MappingProfiles;

public class NoteMappingProfile : Profile
{
    public NoteMappingProfile()
    {
        CreateMap<Note, NoteResponse>();
        CreateMap<CreateNote, Note>();
        CreateMap<UpdateNote, Note>()
            .ForMember(dest => dest.Content, opt => 
                opt.Condition(src => !string.IsNullOrWhiteSpace(src.Content)));
    }
}