using Microsoft.EntityFrameworkCore;
using Scola360.Academico.Domain.Entities;
using Scola360.Academico.Domain.Interfaces;
using Scola360.Academico.Infrastructure.Data;

namespace Scola360.Academico.Infrastructure.Repositories
{
    public class CurriculoRepository(AppDbContext db) : ICurriculoRepository
    {
        public async Task<Curriculo> AddAsync(Curriculo curriculo, CancellationToken ct)
        {
            db.Set<Curriculo>().Add(curriculo);
            await db.SaveChangesAsync(ct);
            return curriculo;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
        {
            var set = db.Set<Curriculo>();
            var entity = await set.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity is null) return false;
            set.Remove(entity);
            await db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<IEnumerable<Curriculo>> GetAllAsync(CancellationToken ct)
        {
            return await db.Set<Curriculo>().AsNoTracking().ToListAsync(ct);
        }

        public async Task<Curriculo?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await db.Set<Curriculo>().AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task<Curriculo> UpdateAsync(Curriculo curriculo, CancellationToken ct)
        {
            db.Set<Curriculo>().Update(curriculo);
            await db.SaveChangesAsync(ct);
            return curriculo;
        }
    }
}
