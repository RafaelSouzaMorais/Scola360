using Scola360.Academico.Domain.Enums;

namespace Scola360.Academico.Domain.Entities;

public class Aluno
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // Relação com Pessoa (composição)
    public Guid PessoaId { get; set; }
    public Pessoa Pessoa { get; set; } = null!;

    // Certidão
    public string? CertidaoNumero { get; set; }
    public TipoCertidao CertidaoTipo { get; set; }

    // Situação
    public bool Ativo { get; set; } = true;

    // Navegação para responsáveis (join table)
    public ICollection<ResponsavelAluno> ResponsavelAluno { get; set; } = new List<ResponsavelAluno>();
}
