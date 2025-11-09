using Scola360.Academico.Domain.Entities;

namespace Scola360.Academico.Domain.Interfaces
{
    public interface ITurmaRepository
    {
        Task<bool> ExisteTurmaComCodigoAsync(string codigo, CancellationToken ct);
        Task<Turma?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<IEnumerable<Turma>> GetAllAsync(CancellationToken ct);
        Task<Turma> AddAsync(Turma turma, CancellationToken ct);
        Task<Turma> UpdateAsync(Turma turma, CancellationToken ct);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct);
        Task<IEnumerable<Turma>> GetTurmasByCursoCurriculoAsync(Guid? idCurriculo, CancellationToken ct);
    }
}