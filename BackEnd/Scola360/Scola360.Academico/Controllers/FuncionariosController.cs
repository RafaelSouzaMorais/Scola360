using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scola360.Academico.Application.DTOs.Funcionarios;
using Scola360.Academico.Application.Interfaces;

namespace Scola360.Academico.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FuncionariosController(IFuncionarioService service, ILogger<FuncionariosController> logger, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Cadastra um novo funcion?rio.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(FuncionarioReadDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] FuncionarioCreateDto dto, CancellationToken ct)
    {
        try
        {
            var funcionarioReadDto = await service.CreateAsync(dto, ct);
            return Ok(funcionarioReadDto);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("CPF j? cadastrado"))
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza dados de um funcion?rio existente.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(FuncionarioReadDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] FuncionarioUpdateDto dto, CancellationToken ct)
        => Ok(await service.UpdateAsync(id, dto, ct));

    /// <summary>
    /// Retorna um funcion?rio pelo Id.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(FuncionarioReadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var funcionario = await service.GetByIdAsync(id, ct);
        return funcionario is null ? NotFound() : Ok(funcionario);
    }

    /// <summary>
    /// Lista funcion?rios com filtro opcional por nome.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FuncionarioReadDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] string? nome, CancellationToken ct)
        => Ok(await service.GetAsync(nome, ct));

    /// <summary>
    /// Remove um funcion?rio pelo Id.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
    {
        try
        {
            await service.DeleteAsync(id, ct);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Retorna lista de professores para dropdown (apenas Id e Nome Completo).
    /// </summary>
    [HttpGet("professores/dropdown")]
    [ProducesResponseType(typeof(IEnumerable<ProfessorDropdownDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfessoresDropdown(CancellationToken ct)
    {
        try
        {
            var professores = await service.GetProfessoresDropdownAsync(ct);
            return Ok(professores);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao buscar professores para dropdown");
            return StatusCode(500, new { error = "Erro interno do servidor" });
        }
    }
}