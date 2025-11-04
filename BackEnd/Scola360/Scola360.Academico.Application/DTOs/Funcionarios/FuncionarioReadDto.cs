namespace Scola360.Academico.Application.DTOs.Funcionarios;

using Scola360.Academico.Application.DTOs.Common;
using Scola360.Academico.Domain.Enums;

public record class FuncionarioReadDto
{
    public Guid Id { get; init; }
    public Guid PessoaId { get; init; }
    public string NomeCompleto { get; init; } = string.Empty;
    public string CPF { get; init; } = string.Empty;
    public DateTime DataNascimento { get; init; }
    public string? Telefone { get; init; }
    public string? Email { get; init; }
    public CorRaca? CorRaca { get; init; }
    public string? RG { get; init; }
    public Sexo? Sexo { get; init; }
    public string? Nacionalidade { get; init; }
    public string? Naturalidade { get; init; }
    public TipoFuncionario TipoFuncionario { get; init; }
    public Guid UsuarioId { get; init; }
    public string Username { get; init; } = string.Empty;
    public IReadOnlyList<EnderecoReadDto> Enderecos { get; init; } = Array.Empty<EnderecoReadDto>();
}