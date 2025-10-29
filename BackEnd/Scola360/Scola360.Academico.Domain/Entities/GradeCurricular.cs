namespace Scola360.Academico.Domain.Entities
{
    public class GradeCurricular
    {
        public Guid CurriculoId { get; set; }
        public Curriculo Curriculo { get; set; } = null!;
        public Guid DisciplinaId { get; set; }
        public Disciplina Disciplina { get; set; } = null!;

    }
}
