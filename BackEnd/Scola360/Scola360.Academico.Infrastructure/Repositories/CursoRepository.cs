using Microsoft.EntityFrameworkCore;
using Scola360.Academico.Domain.Entities;
using Scola360.Academico.Domain.Interfaces;
using Scola360.Academico.Infrastructure.Data;

namespace Scola360.Academico.Infrastructure.Repositories
{
    public class CursoRepository(AppDbContext db) : ICursoRepository
    {
        public async Task<Curso> AddAsync(Curso curso, CancellationToken ct)
        {
            db.Set<Curso>().Add(curso);
            await db.SaveChangesAsync(ct);
            return curso;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
        {
            var set = db.Set<Curso>();
            var entity = await set.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity is null) return false;
            set.Remove(entity);
            await db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> ExisteCursoComNomeAsync(string nome, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome não pode ser nulo ou vazio.", nameof(nome));
            return await db.Set<Curso>()
                           .AsNoTracking()
                           .AnyAsync(c => c.Nome == nome, ct);
        }

        public async Task<IEnumerable<Curso>> GetAllAsync(CancellationToken ct)
        {
            return await db.Set<Curso>()
                           .AsNoTracking()
                           .ToListAsync(ct);
        }

        public async Task<Curso?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await db.Set<Curso>()
                           .AsNoTracking()
                           .SingleOrDefaultAsync(c => c.Id == id, ct);
        }

        public async Task<Curso> UpdateAsync(Curso curso, CancellationToken ct)
        {
            db.Set<Curso>().Update(curso);
            await db.SaveChangesAsync(ct);
            return curso;
        }
    }
}
