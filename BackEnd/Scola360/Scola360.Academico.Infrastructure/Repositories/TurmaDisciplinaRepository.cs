using Microsoft.EntityFrameworkCore;
using Scola360.Academico.Domain.Entities;
using Scola360.Academico.Domain.Interfaces;
using Scola360.Academico.Infrastructure.Data;

namespace Scola360.Academico.Infrastructure.Repositories
{
    public class TurmaDisciplinaRepository(AppDbContext db) : ITurmaDisciplinaRepository
    {
        public async Task<TurmaDisciplina> AddAsync(TurmaDisciplina turmaDisciplina, CancellationToken ct)
        {
            db.Set<TurmaDisciplina>().Add(turmaDisciplina);
            await db.SaveChangesAsync(ct);
            return turmaDisciplina;
        }

        public async Task<bool> DeleteAsync(Guid turmaDisciplinaId, CancellationToken ct)
        {
            var set = db.Set<TurmaDisciplina>();
            var entity = await set.FirstOrDefaultAsync(x => x.Id == turmaDisciplinaId, ct);
            if (entity is null) return false;

            set.Remove(entity);
            await db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<IEnumerable<TurmaDisciplina>> GetAllAsync(CancellationToken ct)
        {
            return await db.Set<TurmaDisciplina>()
                           .AsNoTracking()
                           .Include(td => td.Turma)
                           .Include(td => td.Disciplina)
                           .Include(td => td.Funcionario)
                                .ThenInclude(f => f.Pessoa)
                           .ToListAsync(ct);
        }

        public async Task<TurmaDisciplina?> GetByIdAsync(Guid turmaDisciplinaId, CancellationToken ct)
        {
            return await db.Set<TurmaDisciplina>()
                           .AsNoTracking()
                           .SingleOrDefaultAsync(td => td.Id == turmaDisciplinaId, ct);
        }

        public async Task<IEnumerable<TurmaDisciplina>> GetByTurmaIdAsync(Guid turmaId, CancellationToken ct)
        {
            return await db.Set<TurmaDisciplina>()
                           .AsNoTracking()
                           .Include(td => td.Turma)
                           .Include(td => td.Disciplina)
                           .Include(td => td.Funcionario)
                                .ThenInclude(f => f.Pessoa)
                           .Where(td => td.TurmaId == turmaId)
                           .ToListAsync(ct);
        }

        public async Task<TurmaDisciplina> UpdateAsync(TurmaDisciplina turmaDisciplina, CancellationToken ct)
        {
            var existing = await db.Set<TurmaDisciplina>()
                .FirstOrDefaultAsync(x => x.Id == turmaDisciplina.Id, ct);
            
            if (existing == null)
                throw new KeyNotFoundException("TurmaDisciplina não encontrada.");

            existing.TurmaId = turmaDisciplina.TurmaId;
            existing.DisciplinaId = turmaDisciplina.DisciplinaId;
            existing.FuncionarioId = turmaDisciplina.FuncionarioId;

            await db.SaveChangesAsync(ct);
            return existing;
        }
    }
}