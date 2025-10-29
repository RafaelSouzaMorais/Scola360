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
}
