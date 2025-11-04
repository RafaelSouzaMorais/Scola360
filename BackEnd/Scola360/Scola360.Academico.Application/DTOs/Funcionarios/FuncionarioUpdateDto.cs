namespace Scola360.Academico.Application.DTOs.Funcionarios;

using Scola360.Academico.Domain.Enums;

public record FuncionarioUpdateDto(
    string NomeCompleto,
    string CPF,
    DateTime DataNascimento,
    string? Email,
    string? Telefone,
    CorRaca CorRaca,
    string? RG,
    Sexo Sexo,
    string? Nacionalidade,
    string? Naturalidade,
    TipoFuncionario TipoFuncionario,
    Guid? pessoaId
);