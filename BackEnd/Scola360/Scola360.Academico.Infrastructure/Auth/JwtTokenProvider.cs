using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Scola360.Academico.Application.Interfaces;
using Scola360.Academico.Domain.Entities;

namespace Scola360.Academico.Infrastructure.Auth;

public class JwtTokenProvider(IConfiguration config) : ITokenProvider
{
    public (string token, DateTime expiresAtUtc) Generate(User user)
    {
        var issuer = config["Jwt:Issuer"] ?? "SistemaAcademico";
        var audience = config["Jwt:Audience"] ?? "SistemaAcademicoAudience";
        var key = config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key não configurado");
        var durationMinutes = int.TryParse(config["Jwt:DurationMinutes"], out var d) ? d : 30;

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Username),
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(durationMinutes);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        return (new JwtSecurityTokenHandler().WriteToken(token), expires);
    }
}
