using Scola360.Academico.Application.DTOs.Turmas;
using Scola360.Academico.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scola360.Academico.Application.Interfaces
{
    public interface ITurmaDisciplinaService
    {
        Task<TurmaDisciplinaReadDto?> GetTurmaDisciplinaByIdAsync(Guid turmaDisciplinaId, CancellationToken ct = default);
        Task<IEnumerable<TurmaDisciplinaReadDto>> GetAllTurmaDisciplinasAsync(CancellationToken ct = default);
        Task<IEnumerable<TurmaDisciplinaReadDto>> GetTurmaDisciplinasByTurmaIdAsync(Guid turmaId, CancellationToken ct = default);
        Task<TurmaDisciplinaReadDto> CreateTurmaDisciplinaAsync(TurmaDisciplinaCreateDto dto, CancellationToken ct = default);
        Task<TurmaDisciplinaReadDto> UpdateTurmaDisciplinaAsync(TurmaDisciplinaUpdateDto dto, CancellationToken ct = default);
        Task<bool> DeleteTurmaDisciplinaAsync(Guid turmaDisciplinaId, CancellationToken ct = default);
    }
}