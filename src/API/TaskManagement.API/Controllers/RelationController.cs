using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Services;
using TaskManagement.DTO.Office.Relation;

namespace TaskManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RelationController(RelationService relationService) : ControllerBase
{
    [HttpGet("office/{officeId:guid}")]
    public async Task<IActionResult> GetRelationsByOffice(Guid officeId)
    {
        var relations = await relationService.GetRelationsByOffice(officeId);
        return Ok(relations);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRelationById(Guid id)
    {
        var relation = await relationService.GetRelationByIdAsync(id);
        if (relation == null)
            return NotFound();

        return Ok(relation);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRelation([FromBody] CreateRelation dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdRelation = await relationService.CreateRelationAsync(dto);
        return CreatedAtAction(nameof(GetRelationById), new { id = createdRelation.Id }, createdRelation);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateRelation(Guid id, [FromBody] UpdateRelation dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != dto.Id)
            return BadRequest("ID mismatch");

        var updated = await relationService.UpdateRelationAsync(dto);
        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRelation(Guid id)
    {
        var deleted = await relationService.DeleteRelationAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}