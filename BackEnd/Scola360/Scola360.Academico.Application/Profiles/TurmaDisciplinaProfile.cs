using AutoMapper;
using Scola360.Academico.Application.DTOs.Turmas;
using Scola360.Academico.Domain.Entities;

namespace Scola360.Academico.Application.Profiles;

public class TurmaDisciplinaProfile : Profile
{
    public TurmaDisciplinaProfile()
    {
        CreateMap<TurmaDisciplina, TurmaDisciplinaReadDto>()
            .ForMember(dest => dest.TurmaCodigo, opt => opt.MapFrom(src => src.Turma.CodigoTurma))
            .ForMember(dest => dest.DisciplinaNome, opt => opt.MapFrom(src => src.Disciplina.Nome))
            .ForMember(dest => dest.FuncionarioNome, opt => opt.MapFrom(src => src.Funcionario.Pessoa.NomeCompleto));
        CreateMap<TurmaDisciplinaUpdateDto, TurmaDisciplina>();
        CreateMap<TurmaDisciplinaCreateDto, TurmaDisciplina>();
        CreateMap<TurmaDisciplinaCreateDto, TurmaDisciplinaUpdateDto>();
    }
}