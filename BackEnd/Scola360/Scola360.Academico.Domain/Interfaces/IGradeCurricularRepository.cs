using Scola360.Academico.Domain.Entities;

namespace Scola360.Academico.Domain.Interfaces
{
    public interface IGradeCurricularRepository
    {
        Task<IEnumerable<GradeCurricular>> GetByCurriculoAsync(Guid curriculoId, CancellationToken ct);
        Task<GradeCurricular> AddAsync(GradeCurricular entity, CancellationToken ct);
        Task<bool> DeleteAsync(Guid curriculoId, Guid disciplinaId, CancellationToken ct);
        Task<IEnumerable<GradeCurricular>> AddBatchAsync(Guid curriculoId, IEnumerable<GradeCurricular> entities, CancellationToken ct);
        Task<IEnumerable<GradeCurricular>> ReplaceAsync(Guid curriculoId, IEnumerable<GradeCurricular> entities, CancellationToken ct);
    }
}
