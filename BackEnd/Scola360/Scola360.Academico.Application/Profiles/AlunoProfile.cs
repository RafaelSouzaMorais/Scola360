using AutoMapper;
using Scola360.Academico.Application.DTOs.Alunos;
using Scola360.Academico.Application.DTOs.Common;
using Scola360.Academico.Application.DTOs.Pessoas;
using Scola360.Academico.Domain.Entities;

namespace Scola360.Academico.Application.Profiles;

public class AlunoProfile : Profile
{
    public AlunoProfile()
    {
        CreateMap<Endereco, EnderecoReadDto>();
        CreateMap<EnderecoCreateDto, Endereco>();
        CreateMap<Pessoa, PessoaReadDto>();

        CreateMap<Aluno, AlunoReadDto>()
            .ForMember(d => d.PessoaId, opt => opt.MapFrom(s => s.PessoaId))
            .ForMember(d => d.NomeCompleto, opt => opt.MapFrom(s => s.Pessoa.NomeCompleto))
            .ForMember(d => d.CPF, opt => opt.MapFrom(s => s.Pessoa.CPF))
            .ForMember(d => d.DataNascimento, opt => opt.MapFrom(s => s.Pessoa.DataNascimento))
            .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Pessoa.Email))
            .ForMember(d => d.CorRaca, opt => opt.MapFrom(s => s.Pessoa.CorRaca))
            .ForMember(d => d.RG, opt => opt.MapFrom(s => s.Pessoa.RG))
            .ForMember(d => d.Sexo, opt => opt.MapFrom(s => s.Pessoa.Sexo))
            .ForMember(d => d.Nacionalidade, opt => opt.MapFrom(s => s.Pessoa.Nacionalidade))
            .ForMember(d => d.Naturalidade, opt => opt.MapFrom(s => s.Pessoa.Naturalidade))
            .ForMember(d => d.CertidaoNumero, opt => opt.MapFrom(s => s.CertidaoNumero))
            .ForMember(d => d.CertidaoTipo, opt => opt.MapFrom(s => s.CertidaoTipo))
            .ForMember(d => d.Enderecos, opt => opt.MapFrom(s => s.Pessoa.Enderecos));

        CreateMap<Aluno, AlunoCompletoReadDto>()
            .ForMember(d => d.PessoaId, opt => opt.MapFrom(s => s.PessoaId))
            .ForMember(d => d.NomeCompleto, opt => opt.MapFrom(s => s.Pessoa.NomeCompleto))
            .ForMember(d => d.CPF, opt => opt.MapFrom(s => s.Pessoa.CPF))
            .ForMember(d => d.DataNascimento, opt => opt.MapFrom(s => s.Pessoa.DataNascimento))
            .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Pessoa.Email))
            .ForMember(d => d.CorRaca, opt => opt.MapFrom(s => s.Pessoa.CorRaca))
            .ForMember(d => d.RG, opt => opt.MapFrom(s => s.Pessoa.RG))
            .ForMember(d => d.Sexo, opt => opt.MapFrom(s => s.Pessoa.Sexo))
            .ForMember(d => d.Nacionalidade, opt => opt.MapFrom(s => s.Pessoa.Nacionalidade))
            .ForMember(d => d.Naturalidade, opt => opt.MapFrom(s => s.Pessoa.Naturalidade))
            .ForMember(d => d.CertidaoNumero, opt => opt.MapFrom(s => s.CertidaoNumero))
            .ForMember(d => d.CertidaoTipo, opt => opt.MapFrom(s => s.CertidaoTipo))
            .ForMember(d => d.Enderecos, opt => opt.MapFrom(s => s.Pessoa.Enderecos))
            .ForMember(d => d.Responsaveis, opt => opt.MapFrom(s => s.ResponsavelAluno.Select(ra => ra.Responsavel)));
    }
}
