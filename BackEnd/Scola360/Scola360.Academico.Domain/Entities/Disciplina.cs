using Scola360.Academico.Domain.Enums;

namespace Scola360.Academico.Domain.Entities
{
    public class Disciplina
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public int CargaHoraria { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public TipoCalculoNota TipoCalculoUnidade { get; set; }
        public TipoCalculoNota TipoCalculoFinal { get; set; }
    }
}
