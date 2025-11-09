using Scola360.Academico.Domain.Entities;

namespace Scola360.Academico.Domain.Interfaces
{
    public interface IDisciplinaRepository
    {
        Task<bool> ExisteDisciplinaComNomeAsync(string nome, CancellationToken ct);
        Task<Disciplina?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<IEnumerable<Disciplina>> GetAllAsync(CancellationToken ct);
        Task<Disciplina> AddAsync(Disciplina disciplina, CancellationToken ct);
        Task<Disciplina> UpdateAsync(Disciplina disciplina, CancellationToken ct);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct);
        Task<IEnumerable<Disciplina>> GetByCurriculoIdAsync(Guid curriculoId, CancellationToken ct);
    }
}
