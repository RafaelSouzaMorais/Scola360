using Scola360.Academico.Application.DTOs.Curriculos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Scola360.Academico.Application.Interfaces
{
    public interface ICurriculoService
    {
        Task<CurriculoReadDto> CurriculoByIdAsync(System.Guid id, CancellationToken ct = default);
        Task<IEnumerable<CurriculoReadDto>> GetAllCurriculosAsync(CancellationToken ct = default);
        Task<CurriculoReadDto> CreateCurriculoAsync(CurriculoCreateDto dto, CancellationToken ct = default);
        Task<CurriculoReadDto> UpdateCurriculoAsync(CurriculoUpdateDto dto, CancellationToken ct = default);
        Task<bool> DeleteCurriculoAsync(System.Guid id, CancellationToken ct = default);
    }
}
