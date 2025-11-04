using Scola360.Academico.Application.DTOs.Funcionarios;

namespace Scola360.Academico.Application.Interfaces;

public interface IFuncionarioService
{
    Task<FuncionarioReadDto> CreateAsync(FuncionarioCreateDto dto, CancellationToken ct = default);
    Task<FuncionarioReadDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<FuncionarioReadDto>> GetAsync(string? nome, CancellationToken ct = default);
    Task<FuncionarioReadDto> UpdateAsync(Guid id, FuncionarioUpdateDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}