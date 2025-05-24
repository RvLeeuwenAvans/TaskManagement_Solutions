using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Services;
using TaskManagement.DTO.Office;

namespace TaskManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OfficeController(OfficeService officeService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllOffices()
    {
        var offices = await officeService.GetAllOffices();
        
        return Ok(offices);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOfficeById(Guid id)
    {
        var office = await officeService.GetOfficeByIdAsync(id);
        if (office == null)
            return NotFound();

        return Ok(office);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOffice([FromBody] OfficeCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdOffice = await officeService.CreateOfficeAsync(dto);
        
        return CreatedAtAction(nameof(GetOfficeById), new { id = createdOffice.Id }, createdOffice);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateOffice(Guid id, [FromBody] OfficeUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != dto.Id)
            return BadRequest("ID mismatch");

        var updated = await officeService.UpdateOfficeAsync(dto);
        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteOffice(Guid id)
    {
        var deleted = await officeService.DeleteOfficeAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
