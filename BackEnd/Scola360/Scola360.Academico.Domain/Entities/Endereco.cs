namespace Scola360.Academico.Domain.Entities;

public class Endereco
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PessoaId { get; set; }
    public Pessoa Pessoa { get; set; } = null!;

    public string CEP { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string? Complemento { get; set; }
    public string Bairro { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Pais { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public bool Principal { get; set; }
}
