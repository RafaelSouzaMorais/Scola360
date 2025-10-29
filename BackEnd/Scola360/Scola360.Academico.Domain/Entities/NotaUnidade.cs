using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scola360.Academico.Domain.Entities
{
    public class NotaUnidade
    {
        public Guid Id { get; set; }
        public Guid MatriculaId { get; set; }
        public Matricula Matricula { get; set; } = null!;
        public Guid UnidadeId { get; set; }
        public Unidade Unidade { get; set; } = null!;

        public decimal Valor { get; set; }
    }

}
