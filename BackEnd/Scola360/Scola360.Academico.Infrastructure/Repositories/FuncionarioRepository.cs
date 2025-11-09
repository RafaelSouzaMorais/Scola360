using Microsoft.EntityFrameworkCore;
using Scola360.Academico.Domain.Entities;
using Scola360.Academico.Domain.Enums;
using Scola360.Academico.Domain.Interfaces;
using Scola360.Academico.Infrastructure.Data;

namespace Scola360.Academico.Infrastructure.Repositories;

public class FuncionarioRepository(AppDbContext db) : IFuncionarioRepository
{
    public async Task<Funcionario> AddAsync(Funcionario funcionario, CancellationToken ct = default)
    {
        db.Set<Funcionario>().Add(funcionario);
        await db.SaveChangesAsync(ct);
        return funcionario;
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var set = db.Set<Funcionario>();
        var entity = await set.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return;

        set.Remove(entity);
        await db.SaveChangesAsync(ct);
    }

    public async Task<Funcionario?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await db.Set<Funcionario>()
            .Include(f => f.Pessoa)
            .ThenInclude(p => p.Enderecos)
            .FirstOrDefaultAsync(f => f.Id == id, ct);
    }

    public async Task<IReadOnlyList<Funcionario>> GetByNameAsync(string? nome, CancellationToken ct = default)
    {
        var query = db.Set<Funcionario>()
            .Include(f => f.Pessoa)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(nome))
        {
            query = query.Where(f => f.Pessoa.NomeCompleto.Contains(nome));
        }

        return await query.ToListAsync(ct);
    }

    public async Task<Funcionario?> GetByPessoaIdAsync(Guid pessoaId, CancellationToken ct = default)
    {
        return await db.Set<Funcionario>()
            .Include(f => f.Pessoa)
            .FirstOrDefaultAsync(f => f.PessoaId == pessoaId, ct);
    }

    public async Task UpdateAsync(Funcionario funcionario, CancellationToken ct = default)
    {
        db.Set<Funcionario>().Update(funcionario);
        await db.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<Funcionario>> GetByTipoAsync(TipoFuncionario tipo, CancellationToken ct = default)
    {
        return await db.Set<Funcionario>()
            .Include(f => f.Pessoa)
            .Where(f => f.tipoFuncionario == tipo)
            .AsNoTracking()
            .ToListAsync(ct);
    }
}