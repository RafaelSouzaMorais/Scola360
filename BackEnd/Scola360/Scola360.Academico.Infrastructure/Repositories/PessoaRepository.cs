using Microsoft.EntityFrameworkCore;
using Scola360.Academico.Domain.Entities;
using Scola360.Academico.Domain.Interfaces;
using Scola360.Academico.Infrastructure.Data;

namespace Scola360.Academico.Infrastructure.Repositories;

public class PessoaRepository(AppDbContext db) : IPessoaRepository
{
    public async Task<Pessoa> AddAsync(Pessoa pessoa, CancellationToken ct = default)
    {
        db.Pessoas.Add(pessoa);
        await db.SaveChangesAsync(ct);
        return pessoa;
    }

    public async Task<Pessoa?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await db.Pessoas.AsNoTracking().Include(p => p.Enderecos).FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<Pessoa?> GetByCpfAsync(string cpf, CancellationToken ct = default)
        => await db.Pessoas.AsNoTracking().Include(p => p.Enderecos).FirstOrDefaultAsync(p => p.CPF == cpf, ct);

    public async Task<IReadOnlyList<Pessoa>> GetAsync(string? nome, CancellationToken ct = default)
    {
        var q = db.Pessoas.AsNoTracking().Include(p => p.Enderecos).AsQueryable();
        if (!string.IsNullOrWhiteSpace(nome))
            q = q.Where(p => p.NomeCompleto.Contains(nome));
        return await q.OrderBy(p => p.NomeCompleto).ToListAsync(ct);
    }

    public async Task UpdateAsync(Pessoa pessoa, CancellationToken ct = default)
    {
        db.Pessoas.Update(pessoa);
        await db.SaveChangesAsync(ct);
    }

    public async Task<bool> CpfExistsAsync(string cpf, CancellationToken ct = default)
        => await db.Pessoas.AnyAsync(p => p.CPF == cpf, ct);
}
