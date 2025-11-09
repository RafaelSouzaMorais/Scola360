namespace Scola360.Academico.Application.DTOs.Disciplinas;

public record DisciplinaDropdownDto
{
    public Guid Id { get; init; }
    public string Nome { get; init; } = string.Empty;
    public string Codigo { get; init; } = string.Empty;
}