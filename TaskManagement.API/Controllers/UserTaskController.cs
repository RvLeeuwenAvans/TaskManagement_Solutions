using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Services;
using TaskManagement.DTO.Office.User.Task;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserTaskController(UserTaskService userTaskService) : ControllerBase
{
    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetTasksByUser(Guid userId)
    {
        var tasks = await userTaskService.GetTasksByUser(userId);
        return Ok(tasks);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTaskById(Guid id)
    {
        var task = await userTaskService.GetTaskByIdAsync(id);
        if (task == null)
            return NotFound();

        return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] UserTaskCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdTask = await userTaskService.CreateTaskAsync(dto);
        return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.Id }, createdTask);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UserTaskUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != dto.Id)
            return BadRequest("ID mismatch");

        var updated = await userTaskService.UpdateTaskAsync(dto);
        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var deleted = await userTaskService.DeleteTaskAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}