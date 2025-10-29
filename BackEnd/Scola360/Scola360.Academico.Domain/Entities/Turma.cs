using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scola360.Academico.Domain.Entities
{
    public class Turma
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PeriodoId { get; set; }
        public Periodo Periodo { get; set; } = null!;
        public Guid CurriculoId { get; set; }
        public Curriculo Curriculo { get; set; } = null!;
        public string CodigoTurma { get; set; } = string.Empty;
        public int CapacidadeMaxima { get; set; }
    }
}
