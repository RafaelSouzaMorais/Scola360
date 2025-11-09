using Scola360.Academico.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scola360.Academico.Application.DTOs.Turmas
{
    public record class TurmaReadDto
    {
        public Guid Id { get; set; }
        public Guid PeriodoId { get; set; }
        public string PeriodoNome { get; set; } = string.Empty;
        public Guid CurriculoId { get; set; }
        public string CurriculoNome { get; set; } = string.Empty;
        public string CodigoTurma { get; set; } = string.Empty;
        public int CapacidadeMaxima { get; set; }
        public Turno Turno { get; set; }
    }
}