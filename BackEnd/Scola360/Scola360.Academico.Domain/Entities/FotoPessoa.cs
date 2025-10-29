namespace Scola360.Academico.Domain.Entities;

public class FotoPessoa
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PessoaId { get; set; }
    public Pessoa Pessoa { get; set; } = null!;

    public string ContentType { get; set; } = string.Empty; // image/jpeg, image/png
    public long Length { get; set; }
    public string StoragePath { get; set; } = string.Empty; // caminho/uri no storage
}
