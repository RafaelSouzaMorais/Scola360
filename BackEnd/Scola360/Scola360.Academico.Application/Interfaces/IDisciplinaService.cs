using Scola360.Academico.Application.DTOs.Disciplinas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scola360.Academico.Application.Interfaces
{
    public interface IDisciplinaService
    {
        Task<DisciplinaReadDto> DisciplinaByIdAsync(Guid id, CancellationToken ct = default); 
        Task<IEnumerable<DisciplinaReadDto>> GetAllDisciplinasAsync(CancellationToken ct = default);
        Task<DisciplinaReadDto> CreateDisciplinaAsync(DisciplinaCreateDto dto, CancellationToken ct = default);
        Task<DisciplinaReadDto> UpdateDisciplinaAsync(DisciplinaUpdateDto dto, CancellationToken ct = default);
        Task<bool> DeleteDisciplinaAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<DisciplinaDropdownDto>> GetDisciplinasByCurriculoIdAsync(Guid curriculoId, CancellationToken ct = default);
    }
}
