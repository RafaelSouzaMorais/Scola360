using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scola360.Academico.Domain.Enums
{
    public enum TipoCalculoNota
    {
        MediaSimples = 1,       // (nota1 + nota2 + ...)/n
        MediaPonderada = 2,     // considerando pesos
        MediaPonderadaNota = 3, // considerando pesos e notas
        MaiorNota = 4,           // pega apenas a maior nota
        UltimaNota = 5,          // substitui a anterior
    }
}
