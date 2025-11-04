using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scola360.Academico.Application.DTOs.Pessoas;
using Scola360.Academico.Application.Interfaces;

namespace Scola360.Academico.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PessoasController(IPessoaService service, ILogger<PessoasController> logger, IMapper mapper) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(PessoaReadDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] PessoaCreateDto dto, CancellationToken ct)
    {
        try
        {
            var created = await service.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PessoaReadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var pessoa = await service.GetByIdAsync(id, ct);
        return pessoa is null ? NotFound() : Ok(pessoa);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PessoaReadDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] string? nome, CancellationToken ct)
        => Ok(await service.GetAsync(nome, ct));

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(PessoaReadDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] PessoaUpdateDto dto, CancellationToken ct)
        => Ok(await service.UpdateAsync(id, dto, ct));

    [HttpPost("cpf")]
    [ProducesResponseType(typeof(PessoaReadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> SearchByCpf([FromBody] CpfSearchDto dto, CancellationToken ct)
    {
        if (dto is null || string.IsNullOrWhiteSpace(dto.Cpf))
            return BadRequest(new { error = "CPF obrigatório" });

        var pessoa = await service.GetByCpfAsync(dto.Cpf, ct);
        return pessoa is null ? NotFound() : Ok(pessoa);
    }
}
