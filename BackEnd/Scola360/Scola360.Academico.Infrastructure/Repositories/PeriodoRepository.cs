using Microsoft.EntityFrameworkCore;
using Scola360.Academico.Domain.Entities;
using Scola360.Academico.Domain.Interfaces;
using Scola360.Academico.Infrastructure.Data;

namespace Scola360.Academico.Infrastructure.Repositories;

public class PeriodoRepository(AppDbContext db) : IPeriodoRepository
{
    public async Task<Periodo> AddAsync(Periodo periodo, CancellationToken ct)
    {
        db.Set<Periodo>().Add(periodo);
        await db.SaveChangesAsync(ct);
        return periodo;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var set = db.Set<Periodo>();
        var entity = await set.FirstOrDefaultAsync(p => p.Id == id, ct);
        if (entity is null) return false;
        set.Remove(entity);
        await db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> ExistsByNameYearAsync(string nome, int ano, CancellationToken ct)
        => await db.Set<Periodo>().AsNoTracking().AnyAsync(p => p.Nome == nome && p.Ano == ano, ct);

    public async Task<IReadOnlyList<Periodo>> GetAllAsync(CancellationToken ct)
        => await db.Set<Periodo>().AsNoTracking().OrderBy(p => p.Ano).ThenBy(p => p.Nome).ToListAsync(ct);

    public async Task<Periodo?> GetByIdAsync(Guid id, CancellationToken ct)
        => await db.Set<Periodo>().AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<Periodo> UpdateAsync(Periodo periodo, CancellationToken ct)
    {
        db.Set<Periodo>().Update(periodo);
        await db.SaveChangesAsync(ct);
        return periodo;
    }
}
