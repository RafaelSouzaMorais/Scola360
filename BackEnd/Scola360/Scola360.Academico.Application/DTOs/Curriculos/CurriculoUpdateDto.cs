using System;

namespace Scola360.Academico.Application.DTOs.Curriculos
{
    public class CurriculoUpdateDto
    {
        public Guid Id { get; set; }
        public Guid CursoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
    }
}
