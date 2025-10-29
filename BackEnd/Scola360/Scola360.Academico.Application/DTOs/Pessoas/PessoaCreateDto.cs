namespace Scola360.Academico.Application.DTOs.Pessoas;

using Scola360.Academico.Domain.Enums;

public record PessoaCreateDto(
    Guid Id,
    string NomeCompleto,
    string CPF,
    DateTime DataNascimento,
    string? Email,
    string? Telefone,
    CorRaca CorRaca,
    string? RG,
    Sexo Sexo,
    string? Nacionalidade,
    string? Naturalidade
);
