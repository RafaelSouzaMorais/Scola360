using Scola360.Academico.Application.DTOs.Periodos;

namespace Scola360.Academico.Application.Interfaces;

public interface IPeriodoService
{
    Task<PeriodoReadDto> CreateAsync(PeriodoCreateDto dto, CancellationToken ct = default);
    Task<IReadOnlyList<PeriodoReadDto>> GetAllAsync(CancellationToken ct = default);
    Task<PeriodoReadDto> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<PeriodoReadDto> UpdateAsync(PeriodoUpdateDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
