namespace Scola360.Academico.Domain.Entities
{
    public class ResponsavelAluno
    {
        public Guid AlunoId { get; set; }
        public Aluno Aluno { get; set; } = null!;
        public Guid ResponsavelId { get; set; }
        public Pessoa Responsavel { get; set; } = null!;
    }
}
