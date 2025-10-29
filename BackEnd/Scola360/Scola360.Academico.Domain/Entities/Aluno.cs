using Scola360.Academico.Domain.Enums;

namespace Scola360.Academico.Domain.Entities;

public class Aluno
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // Rela��o com Pessoa (composi��o)
    public Guid PessoaId { get; set; }
    public Pessoa Pessoa { get; set; } = null!;

    // Certid�o
    public string? CertidaoNumero { get; set; }
    public TipoCertidao CertidaoTipo { get; set; }

    // Situa��o
    public bool Ativo { get; set; } = true;

    // Navega��o para respons�veis (join table)
    public ICollection<ResponsavelAluno> ResponsavelAluno { get; set; } = new List<ResponsavelAluno>();
}
