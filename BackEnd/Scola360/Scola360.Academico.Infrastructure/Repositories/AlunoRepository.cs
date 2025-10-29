using Microsoft.EntityFrameworkCore;
using Scola360.Academico.Domain.Entities;
using Scola360.Academico.Domain.Interfaces;
using Scola360.Academico.Infrastructure.Data;

namespace Scola360.Academico.Infrastructure.Repositories;

public class AlunoRepository(AppDbContext db) : IAlunoRepository
{
    public async Task<bool> CpfExistsAsync(string cpf, CancellationToken ct = default)
        => await db.Set<Pessoa>().AnyAsync(p => p.CPF == cpf, ct);

    public async Task<Aluno> AddAsync(Aluno aluno, CancellationToken ct = default)
    {
        db.Set<Aluno>().Add(aluno);
        await db.SaveChangesAsync(ct);
        return await db.Set<Aluno>()
            .AsNoTracking()
            .Include(a => a.Pessoa).ThenInclude(p => p.Enderecos)
            .FirstAsync(a => a.Id == aluno.Id, ct);
    }

    public async Task<Aluno?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await db.Set<Aluno>()
            .AsNoTracking()
            .Include(a => a.Pessoa).ThenInclude(p => p.Enderecos)
            .FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task<IReadOnlyList<Aluno>> GetByNameAsync(string? nome, CancellationToken ct = default)
    {
        var query = db.Set<Aluno>()
            .AsNoTracking()
            .Include(a => a.Pessoa)
            .ThenInclude(p => p.Enderecos)
            .Include(a => a.ResponsavelAluno)
            .ThenInclude(ra => ra.Responsavel)
            .AsQueryable();
        if (!string.IsNullOrWhiteSpace(nome))
            query = query.Where(a => a.Pessoa.NomeCompleto.Contains(nome));
        return await query.OrderBy(a => a.Pessoa.NomeCompleto).ToListAsync(ct);
    }

    public async Task UpdateAsync(Aluno aluno, CancellationToken ct = default)
    {
        db.Alunos.Update(aluno);
        await db.SaveChangesAsync(ct);
    }
}
