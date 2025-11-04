using AutoMapper;
using Microsoft.Extensions.Logging;
using Scola360.Academico.Application.DTOs.Common;
using Scola360.Academico.Application.DTOs.Pessoas;
using Scola360.Academico.Application.Interfaces;
using Scola360.Academico.Domain.Entities;
using Scola360.Academico.Domain.Enums;
using Scola360.Academico.Domain.Interfaces;

namespace Scola360.Academico.Application.Services;

public class PessoaService(IPessoaRepository repo, IMapper mapper, ILogger<PessoaService> logger) : IPessoaService
{
    public async Task<PessoaReadDto> CreateAsync(PessoaCreateDto dto, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(dto.NomeCompleto) || string.IsNullOrWhiteSpace(dto.CPF))
            throw new ArgumentException("Campos obrigatórios ausentes");
        if (dto.CorRaca is < CorRaca.NaoInformado or > CorRaca.Indigena)
            throw new ArgumentException("Cor/Raça inválida");
        if (dto.Sexo is < Sexo.NaoInformado or > Sexo.Outro)
            throw new ArgumentException("Sexo inválido");

        var entity = new Pessoa
        {
            Id = Guid.NewGuid(),
            NomeCompleto = dto.NomeCompleto,
            CPF = dto.CPF,
            DataNascimento = dto.DataNascimento,
            Email = dto.Email,
            Telefone = dto.Telefone,
            CorRaca = dto.CorRaca,
            RG = dto.RG,
            Sexo = dto.Sexo,
            Nacionalidade = dto.Nacionalidade,
            Naturalidade = dto.Naturalidade
        };

        var created = await repo.AddAsync(entity, ct);
        return mapper.Map<PessoaReadDto>(created);
    }

    public async Task<PessoaReadDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await repo.GetByIdAsync(id, ct);
        return entity is null ? null : mapper.Map<PessoaReadDto>(entity);
    }

    public async Task<PessoaReadDto?> GetByCpfAsync(string cpf, CancellationToken ct = default)
    {
        var entity = await repo.GetByCpfAsync(cpf, ct);
        return entity is null ? null : mapper.Map<PessoaReadDto>(entity);
    }

    public async Task<IReadOnlyList<PessoaReadDto>> GetAsync(string? nome, CancellationToken ct = default)
    {
        var list = await repo.GetAsync(nome, ct);
        return list.Select(mapper.Map<PessoaReadDto>).ToList();
    }

    public async Task<PessoaReadDto> UpdateAsync(Guid id, PessoaUpdateDto dto, CancellationToken ct = default)
    {
        var entity = await repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Pessoa não encontrada");
        entity.NomeCompleto = dto.NomeCompleto;
        entity.CPF = dto.CPF;
        entity.DataNascimento = dto.DataNascimento;
        entity.Email = dto.Email;
        entity.Telefone = dto.Telefone;
        entity.CorRaca = dto.CorRaca;
        entity.RG = dto.RG;
        entity.Sexo = dto.Sexo;
        entity.Nacionalidade = dto.Nacionalidade;
        entity.Naturalidade = dto.Naturalidade;
        await repo.UpdateAsync(entity, ct);
        return mapper.Map<PessoaReadDto>(entity);
    }

    public async Task<bool> CpfExistsAsync(string cpf, CancellationToken ct = default)
    {
        return await repo.CpfExistsAsync(cpf, ct);
    }
}
