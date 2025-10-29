using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scola360.Academico.Application.DTOs.Alunos;
using Scola360.Academico.Application.Interfaces;

namespace Scola360.Academico.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AlunosController(IAlunoService service, ILogger<AlunosController> logger, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Cadastra um novo aluno.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(AlunoReadDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] AlunoCreateDto dto, CancellationToken ct)
    {
        try
        {
            var alunoREadDto = await service.CreateAsync(dto, ct);
            return Ok(alunoREadDto);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("CPF já cadastrado"))
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza dados de um aluno existente.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(AlunoReadDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] AlunoUpdateDto dto, CancellationToken ct)
        => Ok(await service.UpdateAsync(id, dto, ct));

    /// <summary>
    /// Associa um responsável a um aluno.
    /// </summary>
    [HttpPut("{id:guid}/responsavel/{responsavelId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SetResponsavel([FromRoute] Guid id, [FromRoute] Guid responsavelId, CancellationToken ct)
    {
        try
        {
            await service.SetResponsavelAsync(id, responsavelId, ct);
            return Ok();
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Aluno ou responsável não encontrado"))
        {
            return NotFound(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }


    /// <summary>
        /// Retorna um aluno pelo Id.
        /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AlunoReadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var aluno = await service.GetByIdAsync(id, ct);
        return aluno is null ? NotFound() : Ok(aluno);
    }

    /// <summary>
    /// Lista alunos com filtro opcional por nome.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AlunoCompletoReadDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] string? nome, CancellationToken ct)
        => Ok(await service.GetAsync(nome, ct));
}
