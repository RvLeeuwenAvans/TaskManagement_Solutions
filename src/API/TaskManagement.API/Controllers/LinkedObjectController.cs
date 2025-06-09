using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Services;
using TaskManagement.DTO.Office.User.Task.LinkedObject;

namespace TaskManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LinkedObjectController(LinkedObjectService linkedObjectService): ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateLinkedObject([FromBody] CreateLinkedObject dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var linkedObject = await linkedObjectService.CreateLinkedObjectAsync(dto);

        return Ok(linkedObject);
    }
}