using AutoMapper;
using Microsoft.Extensions.Logging;
using Scola360.Academico.Application.DTOs.Common;
using Scola360.Academico.Application.Interfaces;
using Scola360.Academico.Domain.Entities;
using Scola360.Academico.Domain.Interfaces;

namespace Scola360.Academico.Application.Services;

public class EnderecoService(IEnderecoRepository repo, IMapper mapper, ILogger<EnderecoService> logger) : IEnderecoService
{
    public async Task<EnderecoReadDto> CreateAsync(EnderecoCreateDto dto, CancellationToken ct = default)
    {
        if (dto.PessoaId == Guid.Empty)
            throw new ArgumentException("PessoaId inválido");
        if (string.IsNullOrWhiteSpace(dto.CEP) || string.IsNullOrWhiteSpace(dto.Logradouro) || string.IsNullOrWhiteSpace(dto.Numero))
            throw new ArgumentException("Campos obrigatórios ausentes");

        var entity = mapper.Map<Endereco>(dto);
        entity.Id = Guid.NewGuid();

        var created = await repo.AddAsync(entity, ct);
        return mapper.Map<EnderecoReadDto>(created);
    }

    public async Task<IReadOnlyList<EnderecoReadDto>> GetByPessoaAsync(Guid pessoaId, CancellationToken ct = default)
    {
        var list = await repo.GetByPessoaAsync(pessoaId, ct);
        return list.Select(mapper.Map<EnderecoReadDto>).ToList();
    }

    public async Task<EnderecoReadDto> UpdateAsync(Guid pessoaId, Guid id, EnderecoUpdateDto dto, CancellationToken ct = default)
    {
        var list = await repo.GetByPessoaAsync(pessoaId, ct);
        var entity = list.FirstOrDefault(e => e.Id == id) ?? throw new KeyNotFoundException("Endereço não encontrado");
        entity.CEP = dto.CEP;
        entity.Logradouro = dto.Logradouro;
        entity.Numero = dto.Numero;
        entity.Complemento = dto.Complemento;
        entity.Bairro = dto.Bairro;
        entity.Cidade = dto.Cidade;
        entity.Estado = dto.Estado;
        entity.Pais = dto.Pais;
        entity.Tipo = dto.Tipo;
        entity.Principal = dto.Principal;
        // Persistir
        // Idealmente repo.UpdateAsync, usando o mesmo contexto tracking. Aqui reanexamos:
        var updated = await repo.AddAsync(entity, ct); // workaround: em um repo real teríamos Update
        return mapper.Map<EnderecoReadDto>(updated);
    }
}
