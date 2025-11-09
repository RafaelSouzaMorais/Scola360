using AutoMapper;
using Scola360.Academico.Application.DTOs.Turmas;
using Scola360.Academico.Domain.Entities;

namespace Scola360.Academico.Application.Profiles;

public class TurmaProfile : Profile
{
    public TurmaProfile()
    {
        CreateMap<Turma, TurmaReadDto>()
            .ForMember(dest => dest.PeriodoNome, opt => opt.MapFrom(src => src.Periodo.Nome))
            .ForMember(dest => dest.CurriculoNome, opt => opt.MapFrom(src => src.Curriculo.Nome));
        CreateMap<TurmaUpdateDto, Turma>();
        CreateMap<TurmaCreateDto, Turma>();
    }
}