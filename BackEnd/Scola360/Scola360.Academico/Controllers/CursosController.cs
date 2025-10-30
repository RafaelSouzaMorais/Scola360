using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scola360.Academico.Application.DTOs.Cursos;
using Scola360.Academico.Application.Interfaces;

namespace Scola360.Academico.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CursosController(ICursoService service, ILogger<CursosController> logger, IMapper mapper) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            try
            {
                var cursos = await service.GetAllCursosAsync(ct);
                if (cursos == null || !cursos.Any())
                    return NotFound(new { message = "Nenhum curso encontrado" });
                return Ok(cursos);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao obter cursos");
                return StatusCode(500, new { error = "Erro interno do servidor" });
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        {
            try
            {
                var curso = await service.CursoByIdAsync(id, ct);
                return Ok(curso);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Curso não encontrado" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CursoCreateDto dto, CancellationToken ct)
        {
            try
            {
                var result = await service.CreateCursoAsync(dto, ct);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Erro ao criar curso");
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CursoUpdateDto dto, CancellationToken ct)
        {
            try
            {
                var result = await service.UpdateCursoAsync(dto, ct);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Erro ao atualizar curso");
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        {
            try
            {
                await service.DeleteCursoAsync(id, ct);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Erro ao deletar curso");
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
