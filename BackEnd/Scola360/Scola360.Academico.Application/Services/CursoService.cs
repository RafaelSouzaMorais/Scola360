using AutoMapper;
using Microsoft.Extensions.Logging;
using Scola360.Academico.Application.DTOs.Cursos;
using Scola360.Academico.Application.Interfaces;
using Scola360.Academico.Domain.Entities;
using Scola360.Academico.Domain.Interfaces;

namespace Scola360.Academico.Application.Services
{
    public class CursoService(ICursoRepository repo, IMapper mapper, ILogger<CursoService> logger) : ICursoService
    {
        public async Task<CursoReadDto> CreateCursoAsync(CursoCreateDto dto, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome))
                throw new ArgumentException("O nome do curso não pode ser vazio.");

            var entity = mapper.Map<Curso>(dto);
            var created = await repo.AddAsync(entity, ct);
            return mapper.Map<CursoReadDto>(created);
        }

        public async Task<bool> DeleteCursoAsync(Guid id, CancellationToken ct = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("O ID do curso é inválido.");
            return await repo.DeleteAsync(id, ct);
        }

        public async Task<IEnumerable<CursoReadDto>> GetAllCursosAsync(CancellationToken ct = default)
        {
            var list = await repo.GetAllAsync(ct);
            return mapper.Map<IEnumerable<CursoReadDto>>(list);
        }

        public async Task<CursoReadDto> CursoByIdAsync(Guid id, CancellationToken ct = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("O ID do curso é inválido.");
            var entity = await repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Curso não encontrado");
            return mapper.Map<CursoReadDto>(entity);
        }

        public async Task<CursoReadDto> UpdateCursoAsync(CursoUpdateDto dto, CancellationToken ct = default)
        {
            if (dto.Id == Guid.Empty)
                throw new ArgumentException("O ID do curso é inválido.");
            var updated = await repo.UpdateAsync(mapper.Map<Curso>(dto), ct);
            return mapper.Map<CursoReadDto>(updated);
        }
    }
}
