using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scola360.Academico.Application.DTOs.Curriculos;
using Scola360.Academico.Application.DTOs.Grades;
using Scola360.Academico.Application.Interfaces;

namespace Scola360.Academico.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CurriculosController(ICurriculoService curriculoService, IGradeCurricularService gradeService, ILogger<CurriculosController> logger, IMapper mapper) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            try
            {
                var items = await curriculoService.GetAllCurriculosAsync(ct);
                if (items == null || !items.Any()) return NotFound(new { message = "Nenhum currículo encontrado" });
                return Ok(items);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao obter currículos");
                return StatusCode(500, new { error = "Erro interno do servidor" });
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        {
            try
            {
                var item = await curriculoService.CurriculoByIdAsync(id, ct);
                return Ok(item);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Currículo não encontrado" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CurriculoCreateDto dto, CancellationToken ct)
        {
            try
            {
                var created = await curriculoService.CreateCurriculoAsync(dto, ct);
                return Ok(created);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Erro ao criar currículo");
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CurriculoUpdateDto dto, CancellationToken ct)
        {
            try
            {
                var updated = await curriculoService.UpdateCurriculoAsync(dto, ct);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Currículo não encontrado" });
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Erro ao atualizar currículo");
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        {
            try
            {
                await curriculoService.DeleteCurriculoAsync(id, ct);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Erro ao deletar currículo");
                return BadRequest(new { error = ex.Message });
            }
        }

        // Grade Curricular endpoints
        [HttpGet("{curriculoId:guid}/grade")]
        public async Task<IActionResult> GetGrade([FromRoute] Guid curriculoId, CancellationToken ct)
        {
            try
            {
                var itens = await gradeService.GetByCurriculoAsync(curriculoId, ct);
                return Ok(itens);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("{curriculoId:guid}/grade")]
        public async Task<IActionResult> AddDisciplina([FromRoute] Guid curriculoId, [FromBody] GradeCurricularItemCreateDto dto, CancellationToken ct)
        {
            try
            {
                if (dto.CurriculoId == Guid.Empty) dto.CurriculoId = curriculoId;
                var created = await gradeService.AddDisciplinaAsync(dto, ct);
                return Ok(created);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Erro ao adicionar disciplina ao currículo");
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{curriculoId:guid}/grade/{disciplinaId:guid}")]
        public async Task<IActionResult> RemoveDisciplina([FromRoute] Guid curriculoId, [FromRoute] Guid disciplinaId, CancellationToken ct)
        {
            try
            {
                await gradeService.RemoveDisciplinaAsync(curriculoId, disciplinaId, ct);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Erro ao remover disciplina do currículo");
                return BadRequest(new { error = ex.Message });
            }
        }

        // Lote: adicionar várias sem remover existentes
        [HttpPost("{curriculoId:guid}/grade/lote")]
        public async Task<IActionResult> AddGradeBatch([FromRoute] Guid curriculoId, [FromBody] GradeCurricularBatchDto dto, CancellationToken ct)
        {
            try
            {
                var itens = await gradeService.AddDisciplinasBatchAsync(curriculoId, dto.DisciplinaIds, ct);
                return Ok(itens);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Erro ao adicionar disciplinas em lote");
                return BadRequest(new { error = ex.Message });
            }
        }

        // Lote: substituir a grade inteira
        [HttpPut("{curriculoId:guid}/grade/lote")]
        public async Task<IActionResult> ReplaceGrade([FromRoute] Guid curriculoId, [FromBody] GradeCurricularBatchDto dto, CancellationToken ct)
        {
            try
            {
                var itens = await gradeService.ReplaceGradeAsync(curriculoId, dto.DisciplinaIds, ct);
                return Ok(itens);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Erro ao substituir grade");
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
