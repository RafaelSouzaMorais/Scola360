using Scola360.Academico.Domain.Entities;

namespace Scola360.Academico.Domain.Interfaces;

public interface IPeriodoRepository
{
    Task<Periodo?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Periodo>> GetAllAsync(CancellationToken ct = default);
    Task<bool> ExistsByNameYearAsync(string nome, int ano, CancellationToken ct = default);
    Task<Periodo> AddAsync(Periodo periodo, CancellationToken ct = default);
    Task<Periodo> UpdateAsync(Periodo periodo, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
