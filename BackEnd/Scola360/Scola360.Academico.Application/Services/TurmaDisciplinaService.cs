using AutoMapper;
using Microsoft.Extensions.Logging;
using Scola360.Academico.Application.DTOs.Turmas;
using Scola360.Academico.Application.Interfaces;
using Scola360.Academico.Domain.Entities;
using Scola360.Academico.Domain.Interfaces;

namespace Scola360.Academico.Application.Services
{
    public class TurmaDisciplinaService(ITurmaDisciplinaRepository repo, IMapper mapper, ILogger<TurmaDisciplinaService> logger) : ITurmaDisciplinaService
    {
        public async Task<TurmaDisciplinaReadDto> CreateTurmaDisciplinaAsync(TurmaDisciplinaCreateDto dto, CancellationToken ct = default)
        {
            if (dto.TurmaId == Guid.Empty)
                throw new ArgumentException("O ID da turma é inválido.");

            if (dto.DisciplinaId == Guid.Empty)
                throw new ArgumentException("O ID da disciplina é inválido.");

            if (dto.FuncionarioId == Guid.Empty)
                throw new ArgumentException("O ID do funcionário é inválido.");

            var entity = mapper.Map<TurmaDisciplina>(dto);
            var created = await repo.AddAsync(entity, ct);
            return mapper.Map<TurmaDisciplinaReadDto>(created);
        }

        public async Task<TurmaDisciplinaReadDto?> GetTurmaDisciplinaByIdAsync(Guid turmaDisciplinaId, CancellationToken ct = default)
        {
            var entity = await repo.GetByIdAsync(turmaDisciplinaId, ct);
            return entity == null ? null : mapper.Map<TurmaDisciplinaReadDto>(entity);
        }

        public async Task<IEnumerable<TurmaDisciplinaReadDto>> GetAllTurmaDisciplinasAsync(CancellationToken ct = default)
        {
            var entities = await repo.GetAllAsync(ct);
            return mapper.Map<IEnumerable<TurmaDisciplinaReadDto>>(entities);
        }

        public async Task<IEnumerable<TurmaDisciplinaReadDto>> GetTurmaDisciplinasByTurmaIdAsync(Guid turmaId, CancellationToken ct = default)
        {
            if (turmaId == Guid.Empty)
                throw new ArgumentException("O ID da turma é inválido.");
            var entities = await repo.GetByTurmaIdAsync(turmaId, ct);
            return mapper.Map<IEnumerable<TurmaDisciplinaReadDto>>(entities);
        }

        public async Task<TurmaDisciplinaReadDto> UpdateTurmaDisciplinaAsync(TurmaDisciplinaUpdateDto dto, CancellationToken ct = default)
        {
            if (dto.TurmaId == Guid.Empty)
                throw new ArgumentException("O ID da turma é inválido.");

            if (dto.DisciplinaId == Guid.Empty)
                throw new ArgumentException("O ID da disciplina é inválido.");

            if (dto.FuncionarioId == Guid.Empty)
                throw new ArgumentException("O ID do funcionário é inválido.");

            var entity = mapper.Map<TurmaDisciplina>(dto);
            var updated = await repo.UpdateAsync(entity, ct);
            return mapper.Map<TurmaDisciplinaReadDto>(updated);
        }

        public async Task<bool> DeleteTurmaDisciplinaAsync(Guid turmaDisciplinaId, CancellationToken ct = default)
        {
            if (turmaDisciplinaId == Guid.Empty)
                throw new ArgumentException("Os IDs são inválidos.");
            return await repo.DeleteAsync(turmaDisciplinaId, ct);
        }

        public Task<TurmaDisciplinaReadDto?> GetTurmaDisciplinaByIdAsync(Guid turmaId, Guid disciplinaId, Guid funcionarioId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}