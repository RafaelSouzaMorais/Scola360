using Microsoft.EntityFrameworkCore;
using Scola360.Academico.Domain.Entities;
using Scola360.Academico.Domain.Interfaces;
using Scola360.Academico.Infrastructure.Data;

namespace Scola360.Academico.Infrastructure.Repositories;

public class FuncionarioRepository(AppDbContext db) : IFuncionarioRepository
{
    public async Task<Funcionario> AddAsync(Funcionario funcionario, CancellationToken ct = default)
    {
        db.Set<Funcionario>().Add(funcionario);
        await db.SaveChangesAsync(ct);
        return await db.Set<Funcionario>()
            .AsNoTracking()
            .Include(f => f.Pessoa).ThenInclude(p => p.Enderecos)
            .Include(f => f.Usuario)
            .FirstAsync(f => f.Id == funcionario.Id, ct);
    }

    public async Task<Funcionario?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await db.Set<Funcionario>()
            .AsNoTracking()
            .Include(f => f.Pessoa).ThenInclude(p => p.Enderecos)
            .Include(f => f.Usuario)
            .FirstOrDefaultAsync(f => f.Id == id, ct);

    public async Task<Funcionario?> GetByPessoaIdAsync(Guid pessoaId, CancellationToken ct = default)
        => await db.Set<Funcionario>()
            .AsNoTracking()
            .Include(f => f.Pessoa)
            .Include(f => f.Usuario)
            .FirstOrDefaultAsync(f => f.PessoaId == pessoaId, ct);

    public async Task<IReadOnlyList<Funcionario>> GetByNameAsync(string? nome, CancellationToken ct = default)
    {
        var query = db.Set<Funcionario>()
            .AsNoTracking()
            .Include(f => f.Pessoa)
            .ThenInclude(p => p.Enderecos)
            .Include(f => f.Usuario)
            .AsQueryable();
        if (!string.IsNullOrWhiteSpace(nome))
            query = query.Where(f => f.Pessoa.NomeCompleto.Contains(nome));
        return await query.OrderBy(f => f.Pessoa.NomeCompleto).ToListAsync(ct);
    }

    public async Task UpdateAsync(Funcionario funcionario, CancellationToken ct = default)
    {
        db.Funcionarios.Update(funcionario);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await db.Funcionarios.FindAsync(new object[] { id }, ct);
        if (entity != null)
        {
            db.Funcionarios.Remove(entity);
            await db.SaveChangesAsync(ct);
        }
    }
}