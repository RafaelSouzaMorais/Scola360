using AutoMapper;
using Scola360.Academico.Application.DTOs.Periodos;
using Scola360.Academico.Domain.Entities;

namespace Scola360.Academico.Application.Profiles;

public class PeriodoProfile : Profile
{
    public PeriodoProfile()
    {
        CreateMap<Periodo, PeriodoReadDto>();
        CreateMap<PeriodoCreateDto, Periodo>();
        CreateMap<PeriodoUpdateDto, Periodo>();
    }
}
