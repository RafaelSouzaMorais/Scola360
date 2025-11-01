using Microsoft.EntityFrameworkCore;
using Scola360.Academico.Domain.Entities;
using Scola360.Academico.Domain.Interfaces;
using Scola360.Academico.Infrastructure.Data;

namespace Scola360.Academico.Infrastructure.Repositories
{
    public class GradeCurricularRepository(AppDbContext db) : IGradeCurricularRepository
    {
        public async Task<GradeCurricular> AddAsync(GradeCurricular entity, CancellationToken ct)
        {
            db.Set<GradeCurricular>().Add(entity);
            await db.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<IEnumerable<GradeCurricular>> AddBatchAsync(Guid curriculoId, IEnumerable<GradeCurricular> entities, CancellationToken ct)
        {
            var set = db.Set<GradeCurricular>();
            // evita duplicatas: filtra os que já existem
            var incoming = entities.ToList();
            var incomingDiscIds = incoming.Select(e => e.DisciplinaId).ToList();
            var existing = await set.AsNoTracking()
                                    .Where(x => x.CurriculoId == curriculoId && incomingDiscIds.Contains(x.DisciplinaId))
                                    .Select(x => x.DisciplinaId)
                                    .ToListAsync(ct);
            var newOnes = incoming.Where(e => !existing.Contains(e.DisciplinaId)).ToList();
            if (newOnes.Count > 0)
            {
                await set.AddRangeAsync(newOnes, ct);
                await db.SaveChangesAsync(ct);
            }
            return await set.AsNoTracking().Where(x => x.CurriculoId == curriculoId && incomingDiscIds.Contains(x.DisciplinaId)).ToListAsync(ct);
        }

        public async Task<bool> DeleteAsync(Guid curriculoId, Guid disciplinaId, CancellationToken ct)
        {
            var set = db.Set<GradeCurricular>();
            var entity = await set.FirstOrDefaultAsync(x => x.CurriculoId == curriculoId && x.DisciplinaId == disciplinaId, ct);
            if (entity is null) return false;
            set.Remove(entity);
            await db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<IEnumerable<GradeCurricular>> GetByCurriculoAsync(Guid curriculoId, CancellationToken ct)
        {
            return await db.Set<GradeCurricular>()
                           .AsNoTracking()
                           .Where(x => x.CurriculoId == curriculoId)
                           .ToListAsync(ct);
        }

        public async Task<IEnumerable<GradeCurricular>> ReplaceAsync(Guid curriculoId, IEnumerable<GradeCurricular> entities, CancellationToken ct)
        {
            var set = db.Set<GradeCurricular>();
            var current = await set.Where(x => x.CurriculoId == curriculoId).ToListAsync(ct);
            var target = entities.ToList();

            // determina remoções e inserções
            var targetDiscIds = target.Select(t => t.DisciplinaId).ToHashSet();
            var currentDiscIds = current.Select(c => c.DisciplinaId).ToHashSet();

            var toRemove = current.Where(c => !targetDiscIds.Contains(c.DisciplinaId)).ToList();
            var toAdd = target.Where(t => !currentDiscIds.Contains(t.DisciplinaId)).ToList();

            if (toRemove.Count > 0) set.RemoveRange(toRemove);
            if (toAdd.Count > 0) await set.AddRangeAsync(toAdd, ct);

            await db.SaveChangesAsync(ct);

            return await set.AsNoTracking().Where(x => x.CurriculoId == curriculoId).ToListAsync(ct);
        }
    }
}
