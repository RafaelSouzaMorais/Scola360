namespace Scola360.Academico.Domain.Entities
{
    public class Curriculo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CursoId { get; set; }
        public Curso Curso { get; set; } = null!;
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public ICollection<Unidade> Unidades { get; set; } = new List<Unidade>();
        public ICollection<GradeCurricular> GradeCurricular { get; set; } = new List<GradeCurricular>();


    }
}
