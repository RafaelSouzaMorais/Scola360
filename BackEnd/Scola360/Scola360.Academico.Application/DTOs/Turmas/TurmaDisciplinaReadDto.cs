using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scola360.Academico.Application.DTOs.Turmas
{
    public record TurmaDisciplinaReadDto
    {
        public Guid Id { get; set; }
        public Guid TurmaId { get; set; }
        public string TurmaCodigo { get; set; } = string.Empty;
        public Guid DisciplinaId { get; set; }
        public string DisciplinaNome { get; set; } = string.Empty;
        public Guid FuncionarioId { get; set; }
        public string FuncionarioNome { get; set; } = string.Empty;
    }
}