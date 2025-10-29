using AutoMapper;
using Microsoft.Extensions.Logging;
using Scola360.Academico.Application.DTOs.Disciplinas;
using Scola360.Academico.Application.Interfaces;
using Scola360.Academico.Domain.Entities;
using Scola360.Academico.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scola360.Academico.Application.Services
{
    public class DisciplinaService(IDisciplinaRepository repo, IMapper mapper, ILogger<AlunoService> logger) : IDisciplinaService
    {
        public async Task<DisciplinaReadDto> CreateDisciplinaAsync(DisciplinaCreateDto dto, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome))
                throw new ArgumentException("O nome da disciplina não pode ser vazio.");

            if (string.IsNullOrWhiteSpace(dto.Codigo))
                throw new ArgumentException("O código da disciplina não pode ser vazio.");

            if (dto.CargaHoraria <= 0)
                throw new ArgumentException("A carga horária deve ser maior que zero.");

            if (dto.TipoCalculoUnidade == 0)
                throw new ArgumentException("O tipo de cálculo da unidade deve ser válido.");

            if (dto.TipoCalculoFinal == 0)
                throw new ArgumentException("O tipo de cálculo final deve ser válido.");

            return await repo.AddAsync(mapper.Map<Disciplina>(dto), ct)
                .ContinueWith(t => mapper.Map<DisciplinaReadDto>(t.Result), ct);

        }

        public async Task<DisciplinaReadDto> DisciplinaByIdAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DisciplinaReadDto>> GetAllDisciplinasAsync(CancellationToken ct = default)
        {
            return await repo.GetAllAsync(ct)
                .ContinueWith(t => mapper.Map<IEnumerable<DisciplinaReadDto>>(t.Result), ct);
        }

        public async Task<DisciplinaReadDto> UpdateDisciplinaAsync(DisciplinaUpdateDto dto, CancellationToken ct = default)
        {
            if (dto.Id == Guid.Empty)
                throw new ArgumentException("O ID da disciplina é inválido.");

            return await repo.UpdateAsync(mapper.Map<Disciplina>(dto), ct)
                .ContinueWith(t => mapper.Map<DisciplinaReadDto>(t.Result), ct);
        }
    }
}
