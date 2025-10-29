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
}
