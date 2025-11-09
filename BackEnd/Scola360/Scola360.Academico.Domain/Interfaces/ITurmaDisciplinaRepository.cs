using Scola360.Academico.Domain.Entities;

namespace Scola360.Academico.Domain.Interfaces
{
    public interface ITurmaDisciplinaRepository
    {
        Task<TurmaDisciplina?> GetByIdAsync(Guid turmaDisciplinaId, CancellationToken ct);
        Task<IEnumerable<TurmaDisciplina>> GetAllAsync(CancellationToken ct);
        Task<IEnumerable<TurmaDisciplina>> GetByTurmaIdAsync(Guid turmaId, CancellationToken ct);
        Task<TurmaDisciplina> AddAsync(TurmaDisciplina turmaDisciplina, CancellationToken ct);
        Task<TurmaDisciplina> UpdateAsync(TurmaDisciplina turmaDisciplina, CancellationToken ct);
        Task<bool> DeleteAsync(Guid turmaDisciplinaId, CancellationToken ct);
    }
}