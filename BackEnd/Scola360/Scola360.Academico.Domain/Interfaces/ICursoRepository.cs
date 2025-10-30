using Scola360.Academico.Domain.Entities;

namespace Scola360.Academico.Domain.Interfaces
{
    public interface ICursoRepository
    {
        Task<Curso?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<IEnumerable<Curso>> GetAllAsync(CancellationToken ct);
        Task<Curso> AddAsync(Curso curso, CancellationToken ct);
        Task<Curso> UpdateAsync(Curso curso, CancellationToken ct);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct);
        Task<bool> ExisteCursoComNomeAsync(string nome, CancellationToken ct);
    }
}
