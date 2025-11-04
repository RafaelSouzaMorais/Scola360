using Scola360.Academico.Domain.Entities;

namespace Scola360.Academico.Domain.Interfaces;

public interface IAlunoRepository
{
    Task<Aluno> AddAsync(Aluno aluno, CancellationToken ct = default);
    Task<Aluno?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Aluno>> GetByNameAsync(string? nome, CancellationToken ct = default);
    Task UpdateAsync(Aluno aluno, CancellationToken ct = default);
}
