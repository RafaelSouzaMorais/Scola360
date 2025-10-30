using System;

namespace Scola360.Academico.Application.DTOs.Cursos
{
    public class CursoUpdateDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
    }
}
