using AutoMapper;
using Scola360.Academico.Application.DTOs.Cursos;
using Scola360.Academico.Domain.Entities;

namespace Scola360.Academico.Application.Profiles
{
    public class CursoProfile : Profile
    {
        public CursoProfile()
        {
            CreateMap<Curso, CursoReadDto>();
            CreateMap<CursoUpdateDto, Curso>();
            CreateMap<CursoCreateDto, Curso>();
        }
    }
}
