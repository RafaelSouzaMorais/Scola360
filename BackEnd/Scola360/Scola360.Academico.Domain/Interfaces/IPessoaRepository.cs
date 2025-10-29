using Scola360.Academico.Domain.Entities;

namespace Scola360.Academico.Domain.Interfaces;

public interface IPessoaRepository
{
    Task<Pessoa> AddAsync(Pessoa pessoa, CancellationToken ct = default);
    Task<Pessoa?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Pessoa>> GetAsync(string? nome, CancellationToken ct = default);
    Task UpdateAsync(Pessoa pessoa, CancellationToken ct = default);
}
