using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scola360.Academico.Application.DTOs.Turmas;
using Scola360.Academico.Application.Interfaces;
using Scola360.Academico.Application.Services;

namespace Scola360.Academico.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TurmasController : Controller
{
    private readonly ITurmaService _turmaService;
    private readonly ITurmaDisciplinaService _turmaDisciplinaService;
    private readonly ILogger<TurmasController> _logger;
    private readonly IMapper _mapper;

    public TurmasController(ITurmaService turmaService, ITurmaDisciplinaService turmaDisciplinaService, ILogger<TurmasController> logger, IMapper mapper)
    {
        _turmaService = turmaService;
        _turmaDisciplinaService = turmaDisciplinaService;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        try
        {
            var turmas = await _turmaService.GetAllTurmasAsync(ct);
            if (turmas == null || !turmas.Any())
            {
                return NotFound(new { message = "Nenhuma turma encontrada" });
            }
            return Ok(turmas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter turmas");
            return StatusCode(500, new { error = "Erro interno do servidor" });
        }
    }
    //IdCurso e iDCurriculo opicional
    [HttpGet]
    [Route("by-curso-curriculo")]
    public async Task<IActionResult> GetByCursoCurriculo([FromQuery] Guid? Curriculo, CancellationToken ct)
    {
        try
        {
            var turmas = await _turmaService.GetTurmasByCursoCurriculoAsync(Curriculo, ct);
            if (turmas == null || !turmas.Any())
            {
                return NotFound(new { message = "Nenhuma turma encontrada para os critérios fornecidos" });
            }
            return Ok(turmas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter turmas por curso e currículo");
            return StatusCode(500, new { error = "Erro interno do servidor" });
        }
    }

    [HttpGet("{turmaId}/disciplinas")]
    public async Task<IActionResult> GetTurmaDisciplinasByTurmaId([FromRoute] Guid turmaId, CancellationToken ct)
    {
        try
        {
            var turmasDisciplinas = await _turmaDisciplinaService.GetTurmaDisciplinasByTurmaIdAsync(turmaId, ct);
            if (turmasDisciplinas == null || !turmasDisciplinas.Any())
            {
                return NotFound(new { message = "Nenhuma disciplina encontrada para a turma" });
            }
            return Ok(turmasDisciplinas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter disciplinas da turma");
            return StatusCode(500, new { error = "Erro interno do servidor" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TurmaCreateDto turmaCreateDto, CancellationToken ct)
    {
        try
        {
            var turmaReadDto = await _turmaService.CreateTurmaAsync(turmaCreateDto, ct);

            if (turmaCreateDto.TurmasDisciplinas != null && turmaCreateDto.TurmasDisciplinas.Any())
            {
                foreach (var turmaDisciplinaDto in turmaCreateDto.TurmasDisciplinas)
                {
                    turmaDisciplinaDto.TurmaId = turmaReadDto.Id;
                    await _turmaDisciplinaService.CreateTurmaDisciplinaAsync(turmaDisciplinaDto, ct);
                }
            }

            return Ok(turmaReadDto);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Erro ao criar turma");
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] TurmaUpdateDto turmaUpdateDto, CancellationToken ct)
    {
        try
        {
            var turmaReadDto = await _turmaService.UpdateTurmaAsync(turmaUpdateDto, ct);
                return Ok(turmaReadDto);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Erro ao atualizar turma");
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
    {
        try
        {
            await _turmaService.DeleteTurmaAsync(id, ct);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Erro ao deletar turma");
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete]
    [Route("disciplina/{TurmaDisciplinaId:guid}")]
    public async Task<IActionResult> DeleteTurmaDisciplina([FromRoute] Guid TurmaDisciplinaId, CancellationToken ct)
    {
        try
        {
            await _turmaDisciplinaService.DeleteTurmaDisciplinaAsync(TurmaDisciplinaId, ct);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Erro ao deletar disciplina da turma");
            return BadRequest(new { error = ex.Message });
        }
    }
}