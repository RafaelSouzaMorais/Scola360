using Scola360.Academico.Application.DTOs.Auth;

namespace Scola360.Academico.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken ct = default);
}
