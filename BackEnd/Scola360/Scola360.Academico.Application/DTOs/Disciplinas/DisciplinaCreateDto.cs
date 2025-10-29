using Scola360.Academico.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scola360.Academico.Application.DTOs.Disciplinas
{
    public record DisciplinaCreateDto
    {
        public string Nome { get; set; } = null!;
        public string Codigo { get; set; } = null!;
        public int CargaHoraria { get; set; }
        public string Descricao { get; set; } = null!;
        public TipoCalculoNota TipoCalculoUnidade { get; set; }
        public TipoCalculoNota TipoCalculoFinal { get; set; }

    }
}
