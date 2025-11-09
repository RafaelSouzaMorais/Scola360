namespace Scola360.Academico.Application.DTOs.Funcionarios;

public record ProfessorDropdownDto
{
    public Guid Id { get; init; }
    public string NomeCompleto { get; init; } = string.Empty;
}