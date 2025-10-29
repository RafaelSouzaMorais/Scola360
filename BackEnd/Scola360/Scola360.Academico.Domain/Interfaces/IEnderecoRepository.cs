using Scola360.Academico.Domain.Entities;

namespace Scola360.Academico.Domain.Interfaces;

public interface IEnderecoRepository
{
    Task<Endereco> AddAsync(Endereco endereco, CancellationToken ct = default);
    Task<IReadOnlyList<Endereco>> GetByPessoaAsync(Guid pessoaId, CancellationToken ct = default);
}
