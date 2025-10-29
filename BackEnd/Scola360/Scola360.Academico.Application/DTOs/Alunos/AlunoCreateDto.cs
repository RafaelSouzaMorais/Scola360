namespace Scola360.Academico.Application.DTOs.Alunos;

using Scola360.Academico.Domain.Enums;

public record AlunoCreateDto(
    string NomeCompleto,
    string CPF,
    DateTime DataNascimento,
    Guid? ResponsavelId,
    string? Email,
    CorRaca CorRaca,
    string? RG,
    Sexo Sexo,
    string? Nacionalidade,
    string? Naturalidade,
    string? CertidaoNumero,
    TipoCertidao CertidaoTipo,
    bool Ativo
);
