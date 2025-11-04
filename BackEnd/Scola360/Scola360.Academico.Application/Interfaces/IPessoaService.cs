using Scola360.Academico.Application.DTOs.Common;
using Scola360.Academico.Application.DTOs.Pessoas;

namespace Scola360.Academico.Application.Interfaces;

public interface IPessoaService
{
    Task<PessoaReadDto> CreateAsync(PessoaCreateDto dto, CancellationToken ct = default);
    Task<PessoaReadDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<PessoaReadDto?> GetByCpfAsync(string cpf, CancellationToken ct = default);
    Task<IReadOnlyList<PessoaReadDto>> GetAsync(string? nome, CancellationToken ct = default);
    Task<PessoaReadDto> UpdateAsync(Guid id, PessoaUpdateDto dto, CancellationToken ct = default);
    Task<bool> CpfExistsAsync(string cpf, CancellationToken ct = default);
}
