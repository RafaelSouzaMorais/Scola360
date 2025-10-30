using AutoMapper;
using Microsoft.Extensions.Logging;
using Scola360.Academico.Application.DTOs.Periodos;
using Scola360.Academico.Application.Interfaces;
using Scola360.Academico.Domain.Entities;
using Scola360.Academico.Domain.Interfaces;

namespace Scola360.Academico.Application.Services;

public class PeriodoService(IPeriodoRepository repo, IMapper mapper, ILogger<PeriodoService> logger) : IPeriodoService
{
    public async Task<PeriodoReadDto> CreateAsync(PeriodoCreateDto dto, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(dto.Nome))
            throw new ArgumentException("O nome do período não pode ser vazio.");
        if (dto.Ano <= 0)
            throw new ArgumentException("O ano deve ser válido.");
        if (dto.DataFim < dto.DataInicio)
            throw new ArgumentException("A data fim não pode ser menor que a data início.");

        var exists = await repo.ExistsByNameYearAsync(dto.Nome, dto.Ano, ct);
        if (exists)
            throw new ArgumentException("Já existe um período com esse nome e ano.");

        var created = await repo.AddAsync(mapper.Map<Periodo>(dto), ct);
        return mapper.Map<PeriodoReadDto>(created);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("O ID do período é inválido.");
        return await repo.DeleteAsync(id, ct);
    }

    public async Task<IReadOnlyList<PeriodoReadDto>> GetAllAsync(CancellationToken ct = default)
    {
        var list = await repo.GetAllAsync(ct);
        return mapper.Map<IReadOnlyList<PeriodoReadDto>>(list);
    }

    public async Task<PeriodoReadDto> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("O ID do período é inválido.");
        var entity = await repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Período não encontrado");
        return mapper.Map<PeriodoReadDto>(entity);
    }

    public async Task<PeriodoReadDto> UpdateAsync(PeriodoUpdateDto dto, CancellationToken ct = default)
    {
        if (dto.Id == Guid.Empty)
            throw new ArgumentException("O ID do período é inválido.");
        if (string.IsNullOrWhiteSpace(dto.Nome))
            throw new ArgumentException("O nome do período não pode ser vazio.");
        if (dto.Ano <= 0)
            throw new ArgumentException("O ano deve ser válido.");
        if (dto.DataFim < dto.DataInicio)
            throw new ArgumentException("A data fim não pode ser menor que a data início.");

        var updated = await repo.UpdateAsync(mapper.Map<Periodo>(dto), ct);
        return mapper.Map<PeriodoReadDto>(updated);
    }
}
