using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Scola360.Academico.Application.DTOs.Funcionarios;
using Scola360.Academico.Application.DTOs.Pessoas;
using Scola360.Academico.Application.Interfaces;
using Scola360.Academico.Domain.Entities;
using Scola360.Academico.Domain.Enums;
using Scola360.Academico.Domain.Interfaces;

namespace Scola360.Academico.Application.Services;

public class FuncionarioService(IFuncionarioRepository repo, IPessoaService pessoaService, IMapper mapper, ILogger<FuncionarioService> logger) : IFuncionarioService
{
    public async Task<FuncionarioReadDto> CreateAsync(FuncionarioCreateDto dto, CancellationToken ct = default)
    {
        // valida??o simples
        if (string.IsNullOrWhiteSpace(dto.NomeCompleto) || string.IsNullOrWhiteSpace(dto.CPF))
            throw new ArgumentException("Campos obrigat?rios ausentes");

        if (dto.CorRaca is < CorRaca.NaoInformado or > CorRaca.Indigena)
            throw new ArgumentException("Cor/Raça inválida");
        if (dto.Sexo is < Sexo.NaoInformado or > Sexo.Outro)
            throw new ArgumentException("Sexo inválido");
        if (dto.TipoFuncionario is < TipoFuncionario.Professor or > TipoFuncionario.Diretor)
            throw new ArgumentException("Tipo de funcionário inválido");

        if (await pessoaService.CpfExistsAsync(dto.CPF, ct) && dto.pessoaId == null)
        {
            logger.LogWarning("CPF já cadastrado: {CPF}", dto.CPF);
            throw new InvalidOperationException("CPF já cadastrado");
        }
        Pessoa pessoa;
        if (dto.pessoaId != null)
        {
            // Verifica se a pessoa existe
            var existingFuncionario = await repo.GetByPessoaIdAsync(dto.pessoaId.Value, ct);
            if (existingFuncionario != null)
            {
                logger.LogWarning("Pessoa com ID {PessoaId} já é um funcionário", dto.pessoaId);
                throw new InvalidOperationException("Pessoa já é um funcionário");
            }
            else
            {
                var pessoaDto = await pessoaService.GetByIdAsync(dto.pessoaId.Value, ct) ?? throw new KeyNotFoundException("Pessoa não encontrada");
                pessoa = mapper.Map<Pessoa>(pessoaDto);
            }
        }
        else
        {
            var pessoaDto = new PessoaCreateDto
            {
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
            pessoa = mapper.Map<Pessoa>(await pessoaService.CreateAsync(pessoaDto, ct));
        }

        var entity = new Funcionario
        {
            Id = Guid.NewGuid(),
            PessoaId = pessoa.Id,
            tipoFuncionario = dto.TipoFuncionario,
        };

        var created = await repo.AddAsync(entity, ct);
        return mapper.Map<FuncionarioReadDto>(created);
    }

    public async Task<FuncionarioReadDto> UpdateAsync(Guid id, FuncionarioUpdateDto dto, CancellationToken ct = default)
    {
        var entity = await repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Funcionário não encontrado");
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
        entity.tipoFuncionario = dto.TipoFuncionario;
        await repo.UpdateAsync(entity, ct);
        return mapper.Map<FuncionarioReadDto>(entity);
    }

    public async Task<FuncionarioReadDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await repo.GetByIdAsync(id, ct);
        return entity is null ? null : mapper.Map<FuncionarioReadDto>(entity);
    }

    public async Task<IReadOnlyList<FuncionarioReadDto>> GetAsync(string? nome, CancellationToken ct = default)
    {
        var list = await repo.GetByNameAsync(nome, ct);
        return list.Select(mapper.Map<FuncionarioReadDto>).ToList();
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await repo.GetByIdAsync(id, ct);
        if (entity == null) throw new KeyNotFoundException("Funcionário não encontrado");
        await repo.DeleteAsync(id, ct);
    }
}