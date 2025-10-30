using Scola360.Academico.Application.DTOs.Cursos;

namespace Scola360.Academico.Application.Interfaces
{
    public interface ICursoService
    {
        Task<CursoReadDto> CursoByIdAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<CursoReadDto>> GetAllCursosAsync(CancellationToken ct = default);
        Task<CursoReadDto> CreateCursoAsync(CursoCreateDto dto, CancellationToken ct = default);
        Task<CursoReadDto> UpdateCursoAsync(CursoUpdateDto dto, CancellationToken ct = default);
        Task<bool> DeleteCursoAsync(Guid id, CancellationToken ct = default);
    }
}
