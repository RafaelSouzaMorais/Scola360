using Scola360.Academico.Application.DTOs.Turmas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scola360.Academico.Application.Interfaces
{
    public interface ITurmaService
    {
        Task<TurmaReadDto> GetTurmaByIdAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<TurmaReadDto>> GetAllTurmasAsync(CancellationToken ct = default);
        Task<TurmaReadDto> CreateTurmaAsync(TurmaCreateDto dto, CancellationToken ct = default);
        Task<TurmaReadDto> UpdateTurmaAsync(TurmaUpdateDto dto, CancellationToken ct = default);
        Task<bool> DeleteTurmaAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<TurmaReadDto>?> GetTurmasByCursoCurriculoAsync(Guid? idCurriculo, CancellationToken ct);
    }
}