using AutoMapper;
using Scola360.Academico.Application.DTOs.Alunos;
using Scola360.Academico.Application.DTOs.Common;
using Scola360.Academico.Application.DTOs.Disciplinas;
using Scola360.Academico.Application.DTOs.Pessoas;
using Scola360.Academico.Domain.Entities;

namespace Scola360.Academico.Application.Profiles;

public class DisciplinaProfile : Profile
{
    public DisciplinaProfile()
    {
        CreateMap<Disciplina, DisciplinaReadDto>();
        CreateMap<DisciplinaUpdateDto, Disciplina>();
        CreateMap<DisciplinaCreateDto, Disciplina>();
    }
}

