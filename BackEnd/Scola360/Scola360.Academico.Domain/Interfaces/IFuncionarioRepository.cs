using Scola360.Academico.Domain.Entities;

namespace Scola360.Academico.Domain.Interfaces;

public interface IFuncionarioRepository
{
    Task<Funcionario> AddAsync(Funcionario funcionario, CancellationToken ct = default);
    Task<Funcionario?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Funcionario?> GetByPessoaIdAsync(Guid pessoaId, CancellationToken ct = default);
    Task<IReadOnlyList<Funcionario>> GetByNameAsync(string? nome, CancellationToken ct = default);
    Task UpdateAsync(Funcionario funcionario, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}