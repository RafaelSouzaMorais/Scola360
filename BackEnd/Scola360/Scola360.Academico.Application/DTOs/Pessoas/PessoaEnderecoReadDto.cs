namespace Scola360.Academico.Application.DTOs.Pessoas;

using Scola360.Academico.Application.DTOs.Common;
using Scola360.Academico.Domain.Enums;

public record class PessoaEnderecoReadDto
{
    public Guid Id {get; init; }
    public string NomeCompleto {get; init; }
    public string CPF {get; init; }
    public DateTime DataNascimento {get; init; }
    public string? Email {get; init; }
    public string? Telefone {get; init; }
    public CorRaca CorRaca {get; init; }
    public string? RG {get; init; }
    public Sexo Sexo {get; init; }
    public string? Nacionalidade {get; init; }
    public string? Naturalidade {get; init; }
    IEnumerable<EnderecoReadDto> Enderecos { get; init; } = Array.Empty<EnderecoReadDto>();
}
