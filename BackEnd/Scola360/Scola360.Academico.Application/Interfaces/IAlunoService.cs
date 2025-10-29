using Scola360.Academico.Application.DTOs.Alunos;

namespace Scola360.Academico.Application.Interfaces;

public interface IAlunoService
{
    Task<AlunoReadDto> CreateAsync(AlunoCreateDto dto, CancellationToken ct = default);
    Task<AlunoCompletoReadDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<AlunoCompletoReadDto>> GetAsync(string? nome, CancellationToken ct = default);
    Task<AlunoReadDto> UpdateAsync(Guid id, AlunoUpdateDto dto, CancellationToken ct = default);
    Task SetResponsavelAsync(Guid id, Guid responsavelId, CancellationToken ct = default);
}
