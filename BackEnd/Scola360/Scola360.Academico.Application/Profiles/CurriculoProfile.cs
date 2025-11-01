using AutoMapper;
using Scola360.Academico.Application.DTOs.Curriculos;
using Scola360.Academico.Application.DTOs.Grades;
using Scola360.Academico.Domain.Entities;

namespace Scola360.Academico.Application.Profiles
{
    public class CurriculoProfile : Profile
    {
        public CurriculoProfile()
        {
            CreateMap<Curriculo, CurriculoReadDto>();
            CreateMap<CurriculoCreateDto, Curriculo>();
            CreateMap<CurriculoUpdateDto, Curriculo>();

            CreateMap<GradeCurricular, GradeCurricularItemReadDto>();
        }
    }
}
