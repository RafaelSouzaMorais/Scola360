namespace Scola360.Academico.Domain.Entities
{
    public class TurmaDisciplina
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid TurmaId { get; set; }
        public Turma Turma { get; set; } = null!;
        public Guid DisciplinaId { get; set; }
        public Disciplina Disciplina { get; set; } = null!;
        public Guid FuncionarioId { get; set; }
        public Funcionario Funcionario { get; set; } = null!;
    }
}
