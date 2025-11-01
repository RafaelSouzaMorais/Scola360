using System;

namespace Scola360.Academico.Application.DTOs.Curriculos
{
    public class CurriculoCreateDto
    {
        public Guid CursoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
    }
}
