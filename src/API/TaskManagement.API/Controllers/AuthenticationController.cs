using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Services;
using TaskManagement.DTO.Office.User.Authentication;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController(TokenService tokenService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("user")]
    public async Task<IActionResult> Login([FromBody] AuthenticationDto loginDto)
    {
        var token = await tokenService.AuthenticateUserAsync(loginDto.Email, loginDto.Password);
        if (token == null)
            return Unauthorized("Invalid credentials");

        return Ok(token);
    }
}