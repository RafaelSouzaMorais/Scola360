namespace Scola360.Academico.Application.DTOs.Common;

public record EnderecoUpdateDto(
    string CEP,
    string Logradouro,
    string Numero,
    string? Complemento,
    string Bairro,
    string Cidade,
    string Estado,
    string Pais,
    string Tipo,
    bool Principal
);
