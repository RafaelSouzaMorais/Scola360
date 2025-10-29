namespace Scola360.Academico.Domain.Entities
{
    public class Periodo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; } = string.Empty;
        public int Ano { get; set; }    
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}
