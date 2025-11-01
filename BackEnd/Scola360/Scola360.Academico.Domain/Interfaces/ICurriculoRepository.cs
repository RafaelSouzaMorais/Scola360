using Scola360.Academico.Domain.Entities;

namespace Scola360.Academico.Domain.Interfaces
{
    public interface ICurriculoRepository
    {
        Task<Curriculo?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<IEnumerable<Curriculo>> GetAllAsync(CancellationToken ct);
        Task<Curriculo> AddAsync(Curriculo curriculo, CancellationToken ct);
        Task<Curriculo> UpdateAsync(Curriculo curriculo, CancellationToken ct);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct);
    }
}
