using Scola360.Academico.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scola360.Academico.Domain.Entities
{
    public class HistoricoEscolar
    {
        public Guid Id { get; set; }
        public Guid MatriculaId { get; set; }
        public Matricula Matricula { get; set; } = null!;
        public SituacaoFinal SituacaoFinal { get; set; }
        public decimal MediaFinal { get; set; }
        public ICollection<HistoricoDisciplina> Disciplinas { get; set; } = new List<HistoricoDisciplina>();
    }
}
