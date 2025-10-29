using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scola360.Academico.Domain.Entities
{
    public class NotaAtividadeAvaliativa
    {
        public Guid Id { get; set; }
        public Guid MatriculaId { get; set; }
        public Matricula Matricula { get; set; } = null!;

        public Guid AtividadeId { get; set; }
        public AtividadeAvaliativa Atividade { get; set; } = null!;

        public decimal Valor { get; set; }
    }

}
