using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scola360.Academico.Application.DTOs.Periodos;
using Scola360.Academico.Application.Interfaces;

namespace Scola360.Academico.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PeriodosController(IPeriodoService service, ILogger<PeriodosController> logger, IMapper mapper) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        try
        {
            var periodos = await service.GetAllAsync(ct);
            if (periodos == null || !periodos.Any())
                return NotFound(new { message = "Nenhum período encontrado" });
            return Ok(periodos);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao obter períodos");
            return StatusCode(500, new { error = "Erro interno do servidor" });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        try
        {
            var periodo = await service.GetByIdAsync(id, ct);
            return Ok(periodo);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Período não encontrado" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PeriodoCreateDto dto, CancellationToken ct)
    {
        try
        {
            var result = await service.CreateAsync(dto, ct);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "Erro ao criar período");
            return BadRequest(new { error = "Erro ao criar período" });
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] PeriodoUpdateDto dto, CancellationToken ct)
    {
        try
        {
            var result = await service.UpdateAsync(dto, ct);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "Erro ao atualizar período");
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
    {
        try
        {
            await service.DeleteAsync(id, ct);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "Erro ao deletar período");
            return BadRequest(new { error = ex.Message });
        }
    }
}
