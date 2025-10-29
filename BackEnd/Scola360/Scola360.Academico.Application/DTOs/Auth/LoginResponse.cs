namespace Scola360.Academico.Application.DTOs.Auth;

public record LoginUserDto(Guid Id, string Name, string? Email, string[] Roles);

public record LoginResponse(string Token, LoginUserDto User);
