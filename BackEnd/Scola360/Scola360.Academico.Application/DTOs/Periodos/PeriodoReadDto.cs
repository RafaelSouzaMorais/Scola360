namespace Scola360.Academico.Application.DTOs.Periodos;

public class PeriodoReadDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Ano { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
}
