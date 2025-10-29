using Microsoft.AspNetCore.Mvc;
using Scola360.Academico.Application.DTOs.Common;
using Scola360.Academico.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Scola360.Academico.Controllers;

[ApiController]
[Route("api/pessoas/{pessoaId:guid}/enderecos")]
[Authorize]
public class EnderecosController(IEnderecoService service, ILogger<EnderecosController> logger) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(EnderecoReadDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromRoute] Guid pessoaId, [FromBody] EnderecoCreateDto dto, CancellationToken ct)
    {
        if (dto.PessoaId != pessoaId)
            return BadRequest(new { error = "PessoaId do corpo difere do da rota" });

        try
        {
            var created = await service.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetByPessoa), new { pessoaId }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(EnderecoReadDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromRoute] Guid pessoaId, [FromRoute] Guid id, [FromBody] EnderecoUpdateDto dto, CancellationToken ct)
    {
        var updated = await service.UpdateAsync(pessoaId, id, dto, ct);
        return Ok(updated);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EnderecoReadDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByPessoa([FromRoute] Guid pessoaId, CancellationToken ct)
        => Ok(await service.GetByPessoaAsync(pessoaId, ct));
}
