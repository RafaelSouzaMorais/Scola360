using Microsoft.EntityFrameworkCore;
using Scola360.Academico.Domain.Entities;
using Scola360.Academico.Domain.Interfaces;
using Scola360.Academico.Infrastructure.Data;

namespace Scola360.Academico.Infrastructure.Repositories
{
    public class DisciplinaRepository(AppDbContext db) : IDisciplinaRepository
    {
        public async Task<Disciplina> AddAsync(Disciplina disciplina, CancellationToken ct)
        {            
            db.Set<Disciplina>().Add(disciplina);
            await db.SaveChangesAsync(ct);
            return disciplina;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
        {
            var set = db.Set<Disciplina>();
            var entity = await set.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity is null) return false;

            set.Remove(entity);
            await db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> ExisteDisciplinaComNomeAsync(string nome, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome não pode ser nulo ou vazio.", nameof(nome));

            return await db.Set<Disciplina>()
                           .AsNoTracking()
                           .AnyAsync(d => d.Nome == nome, ct);
        }

        public async Task<IEnumerable<Disciplina>> GetAllAsync(CancellationToken ct)
        {
            return await db.Set<Disciplina>()
                           .AsNoTracking()
                           .ToListAsync(ct);
        }

        public async Task<Disciplina?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await db.Set<Disciplina>()
                           .AsNoTracking()
                           .SingleOrDefaultAsync(d => d.Id == id, ct);
        }

        public async Task<Disciplina> UpdateAsync(Disciplina disciplina, CancellationToken ct)
        {
            // Se estiver sendo rastreado, EF atualizará os campos modificados; caso contrário, marcará como Modified.
            db.Set<Disciplina>().Update(disciplina);
            await db.SaveChangesAsync(ct);
            return disciplina;
        }

        public async Task<IEnumerable<Disciplina>> GetByCurriculoIdAsync(Guid curriculoId, CancellationToken ct)
        {
            return await db.Set<GradeCurricular>()
                .Where(gc => gc.CurriculoId == curriculoId)
                .Include(gc => gc.Disciplina)
                .Select(gc => gc.Disciplina)
                .AsNoTracking()
                .ToListAsync(ct);
        }
    }
}
