using AutoMapper;
using Microsoft.Extensions.Logging;
using Scola360.Academico.Application.DTOs.Curriculos;
using Scola360.Academico.Application.Interfaces;
using Scola360.Academico.Domain.Entities;
using Scola360.Academico.Domain.Interfaces;

namespace Scola360.Academico.Application.Services
{
    public class CurriculoService(ICurriculoRepository repo, ICursoRepository cursoRepo, IMapper mapper, ILogger<CurriculoService> logger) : ICurriculoService
    {
        public async Task<CurriculoReadDto> CreateCurriculoAsync(CurriculoCreateDto dto, CancellationToken ct = default)
        {
            if (dto.CursoId == Guid.Empty) throw new ArgumentException("CursoId inválido");
            if (string.IsNullOrWhiteSpace(dto.Nome)) throw new ArgumentException("Nome não pode ser vazio");
            if (await cursoRepo.GetByIdAsync(dto.CursoId, ct) is null) throw new ArgumentException("Curso não encontrado");

            var entity = mapper.Map<Curriculo>(dto);
            entity.Id = Guid.NewGuid();
            var created = await repo.AddAsync(entity, ct);
            return mapper.Map<CurriculoReadDto>(created);
        }

        public async Task<bool> DeleteCurriculoAsync(Guid id, CancellationToken ct = default)
        {
            if (id == Guid.Empty) throw new ArgumentException("Id inválido");
            return await repo.DeleteAsync(id, ct);
        }

        public async Task<IEnumerable<CurriculoReadDto>> GetAllCurriculosAsync(CancellationToken ct = default)
        {
            var items = await repo.GetAllAsync(ct);
            return items.Select(mapper.Map<CurriculoReadDto>);
        }

        public async Task<CurriculoReadDto> CurriculoByIdAsync(Guid id, CancellationToken ct = default)
        {
            if (id == Guid.Empty) throw new ArgumentException("Id inválido");
            var entity = await repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Currículo não encontrado");
            return mapper.Map<CurriculoReadDto>(entity);
        }

        public async Task<CurriculoReadDto> UpdateCurriculoAsync(CurriculoUpdateDto dto, CancellationToken ct = default)
        {
            if (dto.Id == Guid.Empty) throw new ArgumentException("Id inválido");
            if (dto.CursoId == Guid.Empty) throw new ArgumentException("CursoId inválido");
            var exists = await repo.GetByIdAsync(dto.Id, ct) ?? throw new KeyNotFoundException("Currículo não encontrado");
            var entity = mapper.Map<Curriculo>(dto);
            var updated = await repo.UpdateAsync(entity, ct);
            return mapper.Map<CurriculoReadDto>(updated);
        }
    }
}
