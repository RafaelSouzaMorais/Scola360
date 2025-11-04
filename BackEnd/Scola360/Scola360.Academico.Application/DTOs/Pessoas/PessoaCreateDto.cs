namespace Scola360.Academico.Application.DTOs.Pessoas;

using Scola360.Academico.Domain.Enums;

public record class PessoaCreateDto()
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string NomeCompleto { get; init; } = string.Empty;
    public string CPF { get; init; } = string.Empty;
    public DateTime DataNascimento { get; init; }
    public string? Email { get; init; }
    public string? Telefone { get; init; }
    public CorRaca CorRaca { get; init; }
    public string? RG {  get; init; }
    public Sexo Sexo { get; init; }
    public string? Nacionalidade { get; init; }
    public string? Naturalidade { get; init; }
};
