using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scola360.Academico.Domain.Entities
{
    public class AtividadeAvaliativa
    {
        public Guid Id { get; set; }
        public Guid UnidadeId { get; set; }
        public Unidade Unidade { get; set; } = null!;

        public string Nome { get; set; } = string.Empty;
        public decimal Peso { get; set; }
        public decimal NotaObtida { get; set; }
    }

}
