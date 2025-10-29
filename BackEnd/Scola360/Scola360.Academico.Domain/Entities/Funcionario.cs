using Scola360.Academico.Domain.Enums;

namespace Scola360.Academico.Domain.Entities;

public class Funcionario
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public TipoFuncionario tipoFuncionario { get; set; }
    public Guid PessoaId { get; set; }
    public Pessoa Pessoa { get; set; } = null!;

    public Guid UsuarioId { get; set; }
    public User Usuario { get; set; } = null!;
}
