using Scola360.Academico.Domain.Enums;

namespace Scola360.Academico.Domain.Entities
{
    public class Matricula
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime DataMatricula { get; set; } = DateTime.UtcNow;
        public Guid AlunoId { get; set; }
        public Aluno Aluno { get; set; } = null!;
        public Guid Turmaid { get; set; }
        public Turma Turma { get; set; } = null!;
        public StatusMatricula Status { get; set; }
    }
}
