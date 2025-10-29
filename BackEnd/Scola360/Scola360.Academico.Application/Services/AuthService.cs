using System.Security.Cryptography;
using System.Text;
using Scola360.Academico.Application.DTOs.Auth;
using Scola360.Academico.Application.Interfaces;
using Scola360.Academico.Domain.Entities;
using Scola360.Academico.Domain.Interfaces;

namespace Scola360.Academico.Application.Services;

public class AuthService(IUserRepository users, ITokenProvider tokens) : IAuthService
{
    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var user = await users.GetByUsernameAsync(request.Username, ct);
        if (user is null)
            throw new KeyNotFoundException("Usuário não encontrado");

        if (!user.Active)
            throw new UnauthorizedAccessException("Usuário inativo");

        if (!VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
            throw new UnauthorizedAccessException("Usuário ou senha inválidos");

        var (token, _) = tokens.Generate(user);

        var roles = user.Roles.Select(r => r.Name).ToArray();
        var dto = new LoginUserDto(
            user.Id,
            user.Username,
            null,
            roles
        );

        return new LoginResponse(token, dto);
    }

    private static bool VerifyPassword(string password, string hash, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);
        using var hmac = new HMACSHA256(saltBytes);
        var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        var computedB64 = Convert.ToBase64String(computed);
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(computedB64),
            Encoding.UTF8.GetBytes(hash));
    }
}
