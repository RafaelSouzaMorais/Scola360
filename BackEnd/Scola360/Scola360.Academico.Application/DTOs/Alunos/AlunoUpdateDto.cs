namespace Scola360.Academico.Application.DTOs.Alunos;

using Scola360.Academico.Domain.Enums;

public record AlunoUpdateDto(
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
    string? CertidaoNumero,
    TipoCertidao CertidaoTipo,
    bool Ativo
);
