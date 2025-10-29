using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scola360.Academico.Domain.Entities
{
    public class Unidade
    {
        public Guid Id { get; set; }
        public Guid CurriculoId { get; set; }
        public Curriculo Curriculo { get; set; } = null!;

        public int Numero { get; set; } // Ex: 1, 2, 3, 4
        public string Nome { get; set; } = string.Empty; // Ex: "1º Bimestre"
        public decimal Peso { get; set; }
        public decimal NotaObtida { get; set; }
    }

}
