using AutoMapper;
using Microsoft.Extensions.Logging;
using Scola360.Academico.Application.DTOs.Turmas;
using Scola360.Academico.Application.Interfaces;
using Scola360.Academico.Domain.Entities;
using Scola360.Academico.Domain.Interfaces;

namespace Scola360.Academico.Application.Services
{
    public class TurmaService(ITurmaRepository repo, ITurmaDisciplinaService repoTurmaDisc, IMapper mapper, ILogger<TurmaService> logger) : ITurmaService
    {
        public async Task<TurmaReadDto> CreateTurmaAsync(TurmaCreateDto dto, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(dto.CodigoTurma))
                throw new ArgumentException("O código da turma não pode ser vazio.");

            if (dto.CapacidadeMaxima <= 0)
                throw new ArgumentException("A capacidade máxima deve ser maior que zero.");

            if (dto.PeriodoId == Guid.Empty)
                throw new ArgumentException("O ID do período é inválido.");

            if (dto.CurriculoId == Guid.Empty)
                throw new ArgumentException("O ID do currículo é inválido.");

            var entity = mapper.Map<Turma>(dto);
            var created = await repo.AddAsync(entity, ct);
            return mapper.Map<TurmaReadDto>(created);
        }

        public async Task<TurmaReadDto> GetTurmaByIdAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await repo.GetByIdAsync(id, ct);
            return entity == null ? throw new KeyNotFoundException("Turma não encontrada.") : mapper.Map<TurmaReadDto>(entity);
        }

        public async Task<IEnumerable<TurmaReadDto>> GetAllTurmasAsync(CancellationToken ct = default)
        {
            var entities = await repo.GetAllAsync(ct);
            return mapper.Map<IEnumerable<TurmaReadDto>>(entities);
        }

        public async Task<TurmaReadDto> UpdateTurmaAsync(TurmaUpdateDto dto, CancellationToken ct = default)
        {
            if (dto.Id == Guid.Empty)
                throw new ArgumentException("O ID da turma é inválido.");

            if (string.IsNullOrWhiteSpace(dto.CodigoTurma))
                throw new ArgumentException("O código da turma não pode ser vazio.");
            if (dto.CapacidadeMaxima <= 0)
                throw new ArgumentException("A capacidade máxima deve ser maior que zero.");
            if (dto.PeriodoId == Guid.Empty)
                throw new ArgumentException("O ID do período é inválido.");
            if (dto.CurriculoId == Guid.Empty)
                throw new ArgumentException("O ID do currículo é inválido.");
            if (await repo.GetByIdAsync(dto.Id, ct) == null)
                throw new KeyNotFoundException("Turma não encontrada.");

            if (dto.TurmasDisciplinas != null && dto.TurmasDisciplinas.Any())
            {
                foreach (var turmaDisciplina in dto.TurmasDisciplinas)
                {
                    //se turma disciplina não existe
                    if (turmaDisciplina.Id == null)
                    {
                        turmaDisciplina.Id = Guid.NewGuid();
                        repoTurmaDisc.CreateTurmaDisciplinaAsync(turmaDisciplina, ct).Wait();
                    }
                    else
                    {
                        var turmaDisciplinaUpdate = mapper.Map<TurmaDisciplinaUpdateDto>(turmaDisciplina);
                        repoTurmaDisc.UpdateTurmaDisciplinaAsync(turmaDisciplinaUpdate, ct).Wait();
                    }
                }
            }

            var entity = mapper.Map<Turma>(dto);
            var updated = await repo.UpdateAsync(entity, ct);
            return mapper.Map<TurmaReadDto>(updated);
        }

        public async Task<bool> DeleteTurmaAsync(Guid id, CancellationToken ct = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("O ID da turma é inválido.");
            return await repo.DeleteAsync(id, ct);
        }

        public async Task<IEnumerable<TurmaReadDto>?> GetTurmasByCursoCurriculoAsync(Guid? idCurriculo, CancellationToken ct)
        {
            if (idCurriculo == null)
                throw new ArgumentException("O ID do currículo não pode ser nulo.");
            var entities =  await repo.GetTurmasByCursoCurriculoAsync( idCurriculo, ct);
            return mapper.Map<IEnumerable<TurmaReadDto>>(entities);
        }
    }
}