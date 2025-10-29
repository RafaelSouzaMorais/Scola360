using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scola360.Academico.Domain.Enums;

namespace Scola360.Academico.Controllers;

[ApiController]
[Route("api/enums")]
[Authorize]
public class EnumsController : ControllerBase
{
    [HttpGet("sexo")]
    public IActionResult GetSexo()
        => Ok(Enum.GetValues<Sexo>()
            .Select(e => new { value = (int)e, name = e.ToString() }));

    [HttpGet("corraca")]
    public IActionResult GetCorRaca()
        => Ok(Enum.GetValues<CorRaca>()
            .Select(e => new { value = (int)e, name = e.ToString() }));

    [HttpGet("tipocertidao")]
    public IActionResult GetTipoCertidao()
        => Ok(Enum.GetValues<TipoCertidao>()
            .Select(e => new { value = (int)e, name = e.ToString() }));

    [HttpGet("statusmatricula")]
    public IActionResult GetStatusMatricula()
        => Ok(Enum.GetValues<StatusMatricula>()
            .Select(e => new { value = (int)e, name = e.ToString() }));

    [HttpGet("tipofuncionario")]
    public IActionResult GetTipoFuncionario()
        => Ok(Enum.GetValues<TipoFuncionario>()
            .Select(e => new { value = (int)e, name = e.ToString() }));

    [HttpGet]
    public IActionResult GetAll()
        => Ok(new
        {
            Sexo = Enum.GetValues<Sexo>()
                .Select(e => new { value = (int)e, name = e.ToString() }),
            CorRaca = Enum.GetValues<CorRaca>()
                .Select(e => new { value = (int)e, name = e.ToString() }),
            TipoCertidao = Enum.GetValues<TipoCertidao>()
                .Select(e => new { value = (int)e, name = e.ToString() }),
            StatusMatricula = Enum.GetValues<StatusMatricula>()
                .Select(e => new { value = (int)e, name = e.ToString() }),
            TipoFuncionario = Enum.GetValues<TipoFuncionario>()
                .Select(e => new { value = (int)e, name = e.ToString() }),
            SituacaoFinal = Enum.GetValues<SituacaoFinal>()
                .Select(e => new { value = (int)e, name = e.ToString() }),
            TipoCalculoNota = Enum.GetValues<TipoCalculoNota>()
                .Select(e => new { value = (int)e, name = e.ToString() })

        });
}
