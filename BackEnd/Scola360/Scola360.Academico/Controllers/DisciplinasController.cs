using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scola360.Academico.Application.DTOs.Disciplinas;
using Scola360.Academico.Application.Interfaces;

namespace Scola360.Academico.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DisciplinasController(IDisciplinaService service, ILogger<DisciplinasController> logger, IMapper mapper) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        try
        {
            var disciplinas = await service.GetAllDisciplinasAsync(ct);
            if (disciplinas == null || !disciplinas.Any())
            {
                return NotFound(new { message = "Nenhuma disciplina encontrada" });
            }
            return Ok(disciplinas);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao obter disciplinas");
            return StatusCode(500, new { error = "Erro interno do servidor" });
        }
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DisciplinaCreateDto disciplinaCreateDto, CancellationToken ct)
    {
        try
        {

            var disciplinnaReadDto = await service.CreateDisciplinaAsync(disciplinaCreateDto, ct);
            return Ok(disciplinnaReadDto);
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "Erro ao criar disciplina");
            return BadRequest(new { error = ex.Message });
        }
    }
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] DisciplinaUpdateDto disciplinaUpdateDto, CancellationToken ct)
    {
        try
        {
            var disciplinnaReadDto = await service.UpdateDisciplinaAsync(disciplinaUpdateDto, ct);
            return Ok(disciplinnaReadDto);
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "Erro ao atualizar disciplina");
            return BadRequest(new { error = ex.Message });
        }
    }
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
    {
        try
        {
            await service.DeleteDisciplinaAsync(id, ct);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "Erro ao deletar disciplina");
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Retorna disciplinas por currículo para dropdown.
    /// </summary>
    [HttpGet("curriculo/{curriculoId:guid}/dropdown")]
    [ProducesResponseType(typeof(IEnumerable<DisciplinaDropdownDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDisciplinasByCurriculoDropdown([FromRoute] Guid curriculoId, CancellationToken ct)
    {
        try
        {
            var disciplinas = await service.GetDisciplinasByCurriculoIdAsync(curriculoId, ct);
            return Ok(disciplinas);
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "Erro ao buscar disciplinas por currículo");
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao buscar disciplinas por currículo");
            return StatusCode(500, new { error = "Erro interno do servidor" });
        }
    }
}
