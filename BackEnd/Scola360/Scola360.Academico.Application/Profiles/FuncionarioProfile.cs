using AutoMapper;
using Scola360.Academico.Application.DTOs.Common;
using Scola360.Academico.Application.DTOs.Funcionarios;
using Scola360.Academico.Domain.Entities;

namespace Scola360.Academico.Application.Profiles;

public class FuncionarioProfile : Profile
{
    public FuncionarioProfile()
    {
        CreateMap<Endereco, EnderecoReadDto>();

        CreateMap<Funcionario, FuncionarioReadDto>()
            .ForMember(d => d.PessoaId, opt => opt.MapFrom(s => s.PessoaId))
            .ForMember(d => d.NomeCompleto, opt => opt.MapFrom(s => s.Pessoa.NomeCompleto))
            .ForMember(d => d.CPF, opt => opt.MapFrom(s => s.Pessoa.CPF))
            .ForMember(d => d.DataNascimento, opt => opt.MapFrom(s => s.Pessoa.DataNascimento))
            .ForMember(d => d.Telefone, opt => opt.MapFrom(s => s.Pessoa.Telefone))
            .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Pessoa.Email))
            .ForMember(d => d.CorRaca, opt => opt.MapFrom(s => s.Pessoa.CorRaca))
            .ForMember(d => d.RG, opt => opt.MapFrom(s => s.Pessoa.RG))
            .ForMember(d => d.Sexo, opt => opt.MapFrom(s => s.Pessoa.Sexo))
            .ForMember(d => d.Nacionalidade, opt => opt.MapFrom(s => s.Pessoa.Nacionalidade))
            .ForMember(d => d.Naturalidade, opt => opt.MapFrom(s => s.Pessoa.Naturalidade))
            .ForMember(d => d.TipoFuncionario, opt => opt.MapFrom(s => s.tipoFuncionario))
            .ForMember(d => d.UsuarioId, opt => opt.MapFrom(s => s.UsuarioId))
            .ForMember(d => d.Enderecos, opt => opt.MapFrom(s => s.Pessoa.Enderecos));
    }
}