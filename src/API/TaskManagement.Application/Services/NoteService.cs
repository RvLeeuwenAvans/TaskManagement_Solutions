using AutoMapper;
using FluentValidation;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Office.User.Task.Note;
using TaskManagement.DTO.Office.User.Task.Note;

namespace TaskManagement.Application.Services;

public class NoteService(
    INoteRepository noteRepository,
    IMapper mapper,
    IValidator<CreateNote> createValidator,
    IValidator<UpdateNote> updateValidator)
{
    public Task<List<NoteResponse>> GetNotesByTask(Guid taskId)
    {
        var notes = noteRepository.GetAll()
            .Where(n => n.TaskId == taskId)
            .ToList();

        var response = mapper.Map<List<NoteResponse>>(notes);
        
        return Task.FromResult(response);
    }

    public async Task<NoteResponse?> GetNoteByIdAsync(Guid id)
    {
        var note = await noteRepository.GetByIdAsync(id);
        return note is null ? null : mapper.Map<NoteResponse>(note);
    }

    public async Task<NoteResponse> CreateNoteAsync(CreateNote dto)
    {
        var validation = await createValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        var note = mapper.Map<Note>(dto);
        await noteRepository.AddAsync(note);
        return mapper.Map<NoteResponse>(note);
    }

    public async Task<bool> UpdateNoteAsync(UpdateNote dto)
    {
        var validation = await updateValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        var note = await noteRepository.GetByIdAsync(dto.Id);
        if (note is null)
            return false;

        mapper.Map(dto, note);
        await noteRepository.UpdateAsync(note);
        return true;
    }

    public async Task<bool> DeleteNoteAsync(Guid id)
    {
        var note = await noteRepository.GetByIdAsync(id);
        if (note is null)
            return false;

        await noteRepository.DeleteAsync(id);
        return true;
    }
}