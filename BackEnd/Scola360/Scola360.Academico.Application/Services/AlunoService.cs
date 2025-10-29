using AutoMapper;
using Microsoft.Extensions.Logging;
using Scola360.Academico.Application.DTOs.Alunos;
using Scola360.Academico.Application.Interfaces;
using Scola360.Academico.Domain.Entities;
using Scola360.Academico.Domain.Enums;
using Scola360.Academico.Domain.Interfaces;

namespace Scola360.Academico.Application.Services;

public class AlunoService(IAlunoRepository repo, IResponsavelAlunoRepository repoResponsavelAluno, IMapper mapper, ILogger<AlunoService> logger) : IAlunoService
{
    public async Task<AlunoReadDto> CreateAsync(AlunoCreateDto dto, CancellationToken ct = default)
    {
        // validação simples
        if (string.IsNullOrWhiteSpace(dto.NomeCompleto) || string.IsNullOrWhiteSpace(dto.CPF))
            throw new ArgumentException("Campos obrigatórios ausentes");

        if (dto.CorRaca is < CorRaca.NaoInformado or > CorRaca.Indigena)
            throw new ArgumentException("Cor/Raça inválida");
        if (dto.Sexo is < Sexo.NaoInformado or > Sexo.Outro)
            throw new ArgumentException("Sexo inválido");

        if (await repo.CpfExistsAsync(dto.CPF, ct))
        {
            logger.LogWarning("CPF já cadastrado: {CPF}", dto.CPF);
            throw new InvalidOperationException("CPF já cadastrado");
        }

        // Monta composição Pessoa + Aluno
        var pessoa = new Pessoa
        {
            Id = Guid.NewGuid(),
            NomeCompleto = dto.NomeCompleto,
            CPF = dto.CPF,
            DataNascimento = dto.DataNascimento,
            Email = dto.Email,
            CorRaca = dto.CorRaca,
            RG = dto.RG,
            Sexo = dto.Sexo,
            Nacionalidade = dto.Nacionalidade,
            Naturalidade = dto.Naturalidade
        };

        var entity = new Aluno
        {
            Id = Guid.NewGuid(),
            PessoaId = pessoa.Id,
            Pessoa = pessoa,
            CertidaoNumero = dto.CertidaoNumero,
            CertidaoTipo = dto.CertidaoTipo,
            Ativo = dto.Ativo
        };

        var responsavelAluno = dto.ResponsavelId.HasValue
            ? new ResponsavelAluno
            {
                AlunoId = entity.Id,
                ResponsavelId = dto.ResponsavelId.Value
            }
            : null;

        var created = await repo.AddAsync(entity, ct);
        return mapper.Map<AlunoReadDto>(created);
    }

    public async Task<AlunoReadDto> UpdateAsync(Guid id, AlunoUpdateDto dto, CancellationToken ct = default)
    {
        var entity = await repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Aluno não encontrado");
        entity.Pessoa.NomeCompleto = dto.NomeCompleto;
        entity.Pessoa.CPF = dto.CPF;
        entity.Pessoa.DataNascimento = dto.DataNascimento;
        entity.Pessoa.Email = dto.Email;
        entity.Pessoa.Telefone = dto.Telefone;
        entity.Pessoa.CorRaca = dto.CorRaca;
        entity.Pessoa.RG = dto.RG;
        entity.Pessoa.Sexo = dto.Sexo;
        entity.Pessoa.Nacionalidade = dto.Nacionalidade;
        entity.Pessoa.Naturalidade = dto.Naturalidade;
        entity.CertidaoNumero = dto.CertidaoNumero;
        entity.CertidaoTipo = dto.CertidaoTipo;
        entity.Ativo = dto.Ativo;
        await repo.UpdateAsync(entity, ct);
        return mapper.Map<AlunoReadDto>(entity);
    }

    public async Task SetResponsavelAsync(Guid id, Guid responsavelId, CancellationToken ct = default)
    {
        var entity = new ResponsavelAluno
        {
            AlunoId = id,
            ResponsavelId = responsavelId
        };
        var created = await repoResponsavelAluno.AddAsync(entity, ct);
        return;
    }

    public async Task<AlunoCompletoReadDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await repo.GetByIdAsync(id, ct);
        return entity is null ? null : mapper.Map<AlunoCompletoReadDto>(entity);
    }

    public async Task<IReadOnlyList<AlunoCompletoReadDto>> GetAsync(string? nome, CancellationToken ct = default)
    {
        var list = await repo.GetByNameAsync(nome, ct);
        return list.Select(mapper.Map<AlunoCompletoReadDto>).ToList();
    }
}
