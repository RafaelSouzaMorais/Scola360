using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using Scola360.Academico.Application.DTOs.Grades;

namespace Scola360.Academico.Application.Interfaces
{
    public interface IGradeCurricularService
    {
        Task<IEnumerable<GradeCurricularItemReadDto>> GetByCurriculoAsync(Guid curriculoId, CancellationToken ct = default);
        Task<GradeCurricularItemReadDto> AddDisciplinaAsync(GradeCurricularItemCreateDto dto, CancellationToken ct = default);
        Task<bool> RemoveDisciplinaAsync(Guid curriculoId, Guid disciplinaId, CancellationToken ct = default);
        Task<IEnumerable<GradeCurricularItemReadDto>> AddDisciplinasBatchAsync(Guid curriculoId, IEnumerable<Guid> disciplinaIds, CancellationToken ct = default);
        Task<IEnumerable<GradeCurricularItemReadDto>> ReplaceGradeAsync(Guid curriculoId, IEnumerable<Guid> disciplinaIds, CancellationToken ct = default);
    }
}
