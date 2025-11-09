using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scola360.Academico.Application.DTOs.Turmas
{
    public record class TurmaDisciplinaCreateDto
    {
        public Guid? Id { get; set; }
        public Guid TurmaId { get; set; }
        public Guid DisciplinaId { get; set; }
        public Guid FuncionarioId { get; set; }
    }
}