using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Office.User.Task.Note;
using TaskManagement.DTO.Office.User.Task.Note;

namespace TaskManagement.Application.Services;

public class NoteService(
    INoteRepository noteRepository,
    IMapper mapper,
    IValidator<NoteCreateDto> createValidator,
    IValidator<NoteUpdateDto> updateValidator)
{
    public async Task<List<NoteResponseDto>> GetAllNotesAsync()
    {
        var notes = await noteRepository.GetAll().ToListAsync();
        return mapper.Map<List<NoteResponseDto>>(notes);
    }

    public async Task<NoteResponseDto?> GetNoteByIdAsync(Guid id)
    {
        var note = await noteRepository.GetByIdAsync(id);
        return note is null ? null : mapper.Map<NoteResponseDto>(note);
    }

    public async Task<NoteResponseDto> CreateNoteAsync(NoteCreateDto dto)
    {
        var validation = await createValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        var note = mapper.Map<Note>(dto);
        await noteRepository.AddAsync(note);
        return mapper.Map<NoteResponseDto>(note);
    }

    public async Task<bool> UpdateNoteAsync(NoteUpdateDto dto)
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