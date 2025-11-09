using Microsoft.EntityFrameworkCore;
using Scola360.Academico.Domain.Entities;
using Scola360.Academico.Domain.Interfaces;
using Scola360.Academico.Infrastructure.Data;

namespace Scola360.Academico.Infrastructure.Repositories
{
    public class TurmaRepository(AppDbContext db) : ITurmaRepository
    {
        public async Task<Turma> AddAsync(Turma turma, CancellationToken ct)
        {
            db.Set<Turma>().Add(turma);
            await db.SaveChangesAsync(ct);
            return turma;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
        {
            var set = db.Set<Turma>();
            var entity = await set.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity is null) return false;

            set.Remove(entity);
            await db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> ExisteTurmaComCodigoAsync(string codigo, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("Código não pode ser nulo ou vazio.", nameof(codigo));

            return await db.Set<Turma>()
                           .AsNoTracking()
                           .AnyAsync(t => t.CodigoTurma == codigo, ct);
        }

        public async Task<IEnumerable<Turma>> GetAllAsync(CancellationToken ct)
        {
            return await db.Set<Turma>()
                           .AsNoTracking()
                           .Include(t => t.Periodo)
                           .Include(t => t.Curriculo)
                           .ToListAsync(ct);
        }

        public async Task<Turma?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await db.Set<Turma>()
                           .AsNoTracking()
                           .Include(t => t.Periodo)
                           .Include(t => t.Curriculo)
                           .SingleOrDefaultAsync(t => t.Id == id, ct);
        }

        public async Task<IEnumerable<Turma>> GetTurmasByCursoCurriculoAsync(Guid? idCurriculo, CancellationToken ct)
        {
            return await db.Set<Turma>().AsNoTracking()
                           .Where(t =>
                                (idCurriculo == null || t.CurriculoId == idCurriculo))
                           .ToListAsync(ct);
        }

        public async Task<Turma> UpdateAsync(Turma turma, CancellationToken ct)
        {
            db.Set<Turma>().Update(turma);
            await db.SaveChangesAsync(ct);
            return turma;
        }
    }
}