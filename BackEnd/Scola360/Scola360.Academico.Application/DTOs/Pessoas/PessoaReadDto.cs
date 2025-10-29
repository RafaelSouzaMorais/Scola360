namespace Scola360.Academico.Application.DTOs.Pessoas;

using Scola360.Academico.Application.DTOs.Common;
using Scola360.Academico.Domain.Enums;

public record PessoaReadDto(
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
