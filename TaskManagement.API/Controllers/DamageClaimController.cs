using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Services;
using TaskManagement.DTO.Office.Relation.DamageClaim;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DamageClaimController(DamageClaimService damageClaimService) : ControllerBase
{
    [HttpGet("office/{id:guid}")]
    public async Task<IActionResult> GetClaimsByPolicy(Guid id)
    {
        var claims = await damageClaimService.GetDamageClaimsByOffice(id);
        return Ok(claims);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetClaimById(Guid id)
    {
        var claim = await damageClaimService.GetDamageClaimByIdAsync(id);
        if (claim == null)
            return NotFound();

        return Ok(claim);
    }

    [HttpPost]
    public async Task<IActionResult> CreateClaim([FromBody] DamageClaimCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdClaim = await damageClaimService.CreateDamageClaimAsync(dto);
        return CreatedAtAction(nameof(GetClaimById), new { id = createdClaim.Id }, createdClaim);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateClaim(Guid id, [FromBody] DamageClaimUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != dto.Id)
            return BadRequest("ID mismatch");

        var updated = await damageClaimService.UpdateDamageClaimAsync(dto);
        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteClaim(Guid id)
    {
        var deleted = await damageClaimService.DeleteDamageClaimAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}