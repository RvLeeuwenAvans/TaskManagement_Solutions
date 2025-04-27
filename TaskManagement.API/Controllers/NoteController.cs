using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Services;
using TaskManagement.DTO.Office.User.Task.Note;

namespace TaskManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NoteController(NoteService noteService) : ControllerBase
{
    [HttpGet("task/{taskId:guid}")]
    public async Task<IActionResult> GetNotesByTask(Guid taskId)
    {
        var notes = await noteService.GetNotesByTask(taskId);
        return Ok(notes);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetNoteById(Guid id)
    {
        var note = await noteService.GetNoteByIdAsync(id);
        if (note == null)
            return NotFound();

        return Ok(note);
    }

    [HttpPost]
    public async Task<IActionResult> CreateNote([FromBody] NoteCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdNote = await noteService.CreateNoteAsync(dto);
        return CreatedAtAction(nameof(GetNoteById), new { id = createdNote.Id }, createdNote);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateNote(Guid id, [FromBody] NoteUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != dto.Id)
            return BadRequest("ID mismatch");

        var updated = await noteService.UpdateNoteAsync(dto);
        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteNote(Guid id)
    {
        var deleted = await noteService.DeleteNoteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}