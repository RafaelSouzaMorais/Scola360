namespace Scola360.Academico.Domain.Entities;

using Scola360.Academico.Domain.Enums;

public class Pessoa
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string NomeCompleto { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }

    public CorRaca CorRaca { get; set; }
    public string? RG { get; set; }
    public Sexo Sexo { get; set; }
    public string? Nacionalidade { get; set; }
    public string? Naturalidade { get; set; }

    public ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();

    public FotoPessoa? Foto { get; set; }
}
