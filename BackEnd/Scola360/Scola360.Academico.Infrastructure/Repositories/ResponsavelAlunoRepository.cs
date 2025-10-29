using Microsoft.EntityFrameworkCore;
using Scola360.Academico.Domain.Entities;
using Scola360.Academico.Domain.Interfaces;
using Scola360.Academico.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scola360.Academico.Infrastructure.Repositories
{

    public class ResponsavelAlunoRepository(AppDbContext db) : IResponsavelAlunoRepository
    {
        public Task<ResponsavelAluno> AddAsync(ResponsavelAluno responsavelAluno, CancellationToken ct = default)
        {
            
                db.ResponsaveisAlunos.Add(responsavelAluno);
                db.SaveChangesAsync(ct);
                return Task.FromResult(responsavelAluno);
        }

        public async Task<ResponsavelAluno?> GetByIdAlunoAsync(Guid id, CancellationToken ct = default)
        {
            return await db.Set<ResponsavelAluno>()
                .AsNoTracking()
                .Include(ra => ra.Responsavel)
                .Include(ra => ra.Aluno)
                .FirstOrDefaultAsync(ra => ra.AlunoId == id, ct);


        }

        public async Task<ResponsavelAluno?> GetByIdResponsavelAsync(Guid id, CancellationToken ct = default)
        {
            return await db.Set<ResponsavelAluno>()
                .AsNoTracking()
                .Include(ra => ra.Responsavel)
                .Include(ra => ra.Aluno)
                .FirstOrDefaultAsync(ra => ra.ResponsavelId == id, ct);
        }
    }
}
