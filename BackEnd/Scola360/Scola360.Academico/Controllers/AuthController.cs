using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scola360.Academico.Application.DTOs.Auth;
using Scola360.Academico.Application.Interfaces;
using Scola360.Academico.Domain.Interfaces;

namespace Scola360.Academico.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IAuthService auth, IUserRepository users) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        try
        {
            var result = await auth.LoginAsync(request, ct);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<LoginUserDto>> Me(CancellationToken ct)
    {
        var username = User.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value
                       ?? User.Identity?.Name;
        if (string.IsNullOrWhiteSpace(username))
            return Unauthorized();

        var user = await users.GetByUsernameAsync(username, ct);
        if (user is null)
            return NotFound();

        var dto = new LoginUserDto(
            user.Id,
            user.Username,
            null,
            user.Roles.Select(r => r.Name).ToArray()
        );
        return Ok(dto);
    }
}
