using Microsoft.EntityFrameworkCore;
using Scola360.Academico.Domain.Entities;
using Scola360.Academico.Domain.Interfaces;
using Scola360.Academico.Infrastructure.Data;

namespace Scola360.Academico.Infrastructure.Repositories;

public class EnderecoRepository(AppDbContext db) : IEnderecoRepository
{
    public async Task<Endereco> AddAsync(Endereco endereco, CancellationToken ct = default)
    {
        db.Enderecos.Add(endereco);
        await db.SaveChangesAsync(ct);
        return endereco;
    }

    public async Task<IReadOnlyList<Endereco>> GetByPessoaAsync(Guid pessoaId, CancellationToken ct = default)
        => await db.Enderecos.AsNoTracking()
            .Where(e => e.PessoaId == pessoaId)
            .OrderByDescending(e => e.Principal)
            .ThenBy(e => e.Tipo)
            .ThenBy(e => e.Logradouro)
            .ToListAsync(ct);
}
