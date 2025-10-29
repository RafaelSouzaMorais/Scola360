using Scola360.Academico.Application.DTOs.Common;
using Scola360.Academico.Application.DTOs.Pessoas;

namespace Scola360.Academico.Application.Interfaces;

public interface IEnderecoService
{
    Task<EnderecoReadDto> CreateAsync(EnderecoCreateDto dto, CancellationToken ct = default);
    Task<IReadOnlyList<EnderecoReadDto>> GetByPessoaAsync(Guid pessoaId, CancellationToken ct = default);
    Task<EnderecoReadDto> UpdateAsync(Guid pessoaId, Guid id, EnderecoUpdateDto dto, CancellationToken ct = default);
}
