using Scola360.Academico.Domain.Entities;

public interface IResponsavelAlunoRepository
{
    Task<ResponsavelAluno> AddAsync(ResponsavelAluno responsavelAluno, CancellationToken ct = default);
    Task<ResponsavelAluno?> GetByIdAlunoAsync(Guid id, CancellationToken ct = default);
    Task<ResponsavelAluno?> GetByIdResponsavelAsync(Guid id, CancellationToken ct = default);
}
