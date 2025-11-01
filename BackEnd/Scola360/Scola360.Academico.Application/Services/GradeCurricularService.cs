using AutoMapper;
using Microsoft.Extensions.Logging;
using Scola360.Academico.Application.DTOs.Grades;
using Scola360.Academico.Application.Interfaces;
using Scola360.Academico.Domain.Entities;
using Scola360.Academico.Domain.Interfaces;

namespace Scola360.Academico.Application.Services
{
    public class GradeCurricularService(IGradeCurricularRepository repo, ICurriculoRepository curriculoRepo, IDisciplinaRepository disciplinaRepo, IMapper mapper, ILogger<GradeCurricularService> logger) : IGradeCurricularService
    {
        public async Task<GradeCurricularItemReadDto> AddDisciplinaAsync(GradeCurricularItemCreateDto dto, CancellationToken ct = default)
        {
            if (dto.CurriculoId == Guid.Empty || dto.DisciplinaId == Guid.Empty)
                throw new ArgumentException("Ids inválidos");

            if (await curriculoRepo.GetByIdAsync(dto.CurriculoId, ct) is null)
                throw new ArgumentException("Currículo não encontrado");
            if (await disciplinaRepo.GetByIdAsync(dto.DisciplinaId, ct) is null)
                throw new ArgumentException("Disciplina não encontrada");

            var entity = new GradeCurricular { CurriculoId = dto.CurriculoId, DisciplinaId = dto.DisciplinaId };
            var created = await repo.AddAsync(entity, ct);
            return mapper.Map<GradeCurricularItemReadDto>(created);
        }

        public async Task<IEnumerable<GradeCurricularItemReadDto>> AddDisciplinasBatchAsync(Guid curriculoId, IEnumerable<Guid> disciplinaIds, CancellationToken ct = default)
        {
            if (curriculoId == Guid.Empty) throw new ArgumentException("Id inválido");
            if (await curriculoRepo.GetByIdAsync(curriculoId, ct) is null)
                throw new ArgumentException("Currículo não encontrado");

            var ids = disciplinaIds?.Distinct().ToList() ?? [];
            if (ids.Count == 0) return [];

            // valida todas as disciplinas
            foreach (var did in ids)
            {
                if (did == Guid.Empty) throw new ArgumentException("DisciplinaId inválido");
                if (await disciplinaRepo.GetByIdAsync(did, ct) is null)
                    throw new ArgumentException($"Disciplina não encontrada: {did}");
            }

            var entities = ids.Select(did => new GradeCurricular { CurriculoId = curriculoId, DisciplinaId = did });
            var created = await repo.AddBatchAsync(curriculoId, entities, ct);
            return created.Select(mapper.Map<GradeCurricularItemReadDto>);
        }

        public async Task<IEnumerable<GradeCurricularItemReadDto>> GetByCurriculoAsync(Guid curriculoId, CancellationToken ct = default)
        {
            if (curriculoId == Guid.Empty) throw new ArgumentException("Id inválido");
            var list = await repo.GetByCurriculoAsync(curriculoId, ct);
            return list.Select(mapper.Map<GradeCurricularItemReadDto>);
        }

        public async Task<IEnumerable<GradeCurricularItemReadDto>> ReplaceGradeAsync(Guid curriculoId, IEnumerable<Guid> disciplinaIds, CancellationToken ct = default)
        {
            if (curriculoId == Guid.Empty) throw new ArgumentException("Id inválido");
            if (await curriculoRepo.GetByIdAsync(curriculoId, ct) is null)
                throw new ArgumentException("Currículo não encontrado");

            var ids = disciplinaIds?.Distinct().ToList() ?? [];

            // valida todas as disciplinas (lista pode ser vazia -> limpar grade)
            foreach (var did in ids)
            {
                if (did == Guid.Empty) throw new ArgumentException("DisciplinaId inválido");
                if (await disciplinaRepo.GetByIdAsync(did, ct) is null)
                    throw new ArgumentException($"Disciplina não encontrada: {did}");
            }

            var entities = ids.Select(did => new GradeCurricular { CurriculoId = curriculoId, DisciplinaId = did });
            var result = await repo.ReplaceAsync(curriculoId, entities, ct);
            return result.Select(mapper.Map<GradeCurricularItemReadDto>);
        }

        public async Task<bool> RemoveDisciplinaAsync(Guid curriculoId, Guid disciplinaId, CancellationToken ct = default)
        {
            if (curriculoId == Guid.Empty || disciplinaId == Guid.Empty)
                throw new ArgumentException("Ids inválidos");
            return await repo.DeleteAsync(curriculoId, disciplinaId, ct);
        }
    }
}
