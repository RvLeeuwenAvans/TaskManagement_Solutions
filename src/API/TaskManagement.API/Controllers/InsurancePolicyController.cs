using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Services;
using TaskManagement.DTO.Office.Relation.InsurancePolicy;

namespace TaskManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class InsurancePolicyController(InsurancePolicyService insurancePolicyService) : ControllerBase
{
    [HttpGet("office/{id:guid}")]
    public async Task<IActionResult> GetInsurancePoliciesByOffice(Guid id)
    {
        var policies = await insurancePolicyService.GetInsurancePoliciesByOffice(id);
        
        return Ok(policies);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPolicyById(Guid id)
    {
        var policy = await insurancePolicyService.GetInsurancePolicyByIdAsync(id);
        
        if (policy == null)
            return NotFound();

        return Ok(policy);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePolicy([FromBody] CreateInsurancePolicy dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdPolicy = await insurancePolicyService.CreateInsurancePolicyAsync(dto);
        return CreatedAtAction(nameof(GetPolicyById), new { id = createdPolicy.Id }, createdPolicy);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdatePolicy(Guid id, [FromBody] UpdateInsurancePolicy dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != dto.Id)
            return BadRequest("ID mismatch");

        var updated = await insurancePolicyService.UpdateInsurancePolicyAsync(dto);
        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletePolicy(Guid id)
    {
        var deleted = await insurancePolicyService.DeleteInsurancePolicyAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}