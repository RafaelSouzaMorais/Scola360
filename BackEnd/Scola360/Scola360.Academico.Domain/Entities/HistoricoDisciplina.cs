using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scola360.Academico.Domain.Entities
{
    public class HistoricoDisciplina
    {
        public Guid Id { get; set; }

        public Guid HistoricoEscolarId { get; set; }
        public HistoricoEscolar HistoricoEscolar { get; set; } = null!;

        public Guid DisciplinaId { get; set; }
        public Disciplina Disciplina { get; set; } = null!;

        public decimal MediaFinal { get; set; }
        public decimal Frequencia { get; set; } // percentual 0–100
        public bool Aprovado { get; set; }

        public int CargaHoraria { get; set; } // horas da disciplina no ano

        public string? Observacao { get; set; } // Ex: "Aprovado por conselho", "Recuperação"
    }

}
