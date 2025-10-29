using Scola360.Academico.Domain.Entities;

namespace Scola360.Academico.Application.Interfaces;

public interface ITokenProvider
{
    (string token, DateTime expiresAtUtc) Generate(User user);
}
