using AutoMapper;
using Scola360.Academico.Application.DTOs.Common;
using Scola360.Academico.Application.DTOs.Pessoas;
using Scola360.Academico.Domain.Entities;

namespace Scola360.Academico.Application.Profiles;

public class PessoaProfile : Profile
{
    public PessoaProfile()
    {
        CreateMap<Endereco, EnderecoReadDto>();
        CreateMap<Pessoa, PessoaReadDto>();
        CreateMap<PessoaReadDto, Pessoa>();
    }
}
