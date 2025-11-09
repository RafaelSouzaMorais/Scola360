using Microsoft.EntityFrameworkCore;
using Scola360.Academico.Domain.Entities;
using Scola360.Academico.Domain.Enums;

namespace Scola360.Academico.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Pessoa> Pessoas => Set<Pessoa>();
    public DbSet<Aluno> Alunos => Set<Aluno>();
    public DbSet<ResponsavelAluno> ResponsaveisAlunos => Set<ResponsavelAluno>();
    public DbSet<Funcionario> Funcionarios => Set<Funcionario>();
    public DbSet<Endereco> Enderecos => Set<Endereco>();
    public DbSet<FotoPessoa> FotoPessoa => Set<FotoPessoa>();
    public DbSet<Periodo> Periodos => Set<Periodo>();
    public DbSet<Turma> Turmas => Set<Turma>();
    public DbSet<TurmaDisciplina> TurmasDisciplinas => Set<TurmaDisciplina>();
    public DbSet<Disciplina> Disciplinas => Set<Disciplina>();
    public DbSet<GradeCurricular> GradesCurriculares => Set<GradeCurricular>();
    public DbSet<Curso> Cursos => Set<Curso>();
    public DbSet<Curriculo> Curriculos => Set<Curriculo>();
    public DbSet<Matricula> Matriculas => Set<Matricula>();
    public DbSet<Unidade> Unidades => Set<Unidade>();
    public DbSet<AtividadeAvaliativa> AtividadesAvaliativas => Set<AtividadeAvaliativa>();
    public DbSet<NotaAtividadeAvaliativa> NotasAtividadesAvaliativas => Set<NotaAtividadeAvaliativa>();
    public DbSet<NotaUnidade> NotasUnidades => Set<NotaUnidade>();
    public DbSet<HistoricoEscolar> HistoricosEscolares => Set<HistoricoEscolar>();
    public DbSet<HistoricoDisciplina> HistoricosDisciplinas => Set<HistoricoDisciplina>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Usuário
        modelBuilder.Entity<User>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Username).IsRequired().HasMaxLength(100);
            b.Property(x => x.PasswordHash).IsRequired();
            b.Property(x => x.PasswordSalt).IsRequired();
            b.Property(x => x.Active).HasDefaultValue(true);
            b.Property(x => x.CreatedAt).HasDefaultValueSql("NOW()");
            b.HasIndex(x => x.Username).IsUnique();
            b.ToTable("User");
        });

        // Role (papéis/perfis)
        modelBuilder.Entity<Role>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Name).IsRequired().HasMaxLength(100);
            b.HasIndex(x => x.Name).IsUnique();
            b.ToTable("Role");
        });

        // Relação User <-> Role (N:N) com PK composta [RolesId, UsersId]
        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<Dictionary<string, object>>(
                "RoleUser",
                j => j.HasOne<Role>().WithMany().HasForeignKey("RolesId").OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<User>().WithMany().HasForeignKey("UsersId").OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("RolesId", "UsersId");
                    j.HasIndex("UsersId");
                    j.ToTable("RoleUser");
                });

        // Pessoa
        modelBuilder.Entity<Pessoa>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.NomeCompleto).IsRequired().HasMaxLength(200);
            b.Property(x => x.CPF).IsRequired().HasMaxLength(11);
            b.Property(x => x.Email).HasMaxLength(200);
            b.Property(x => x.Telefone).HasMaxLength(30);
            b.Property(x => x.CorRaca).HasConversion<int>().IsRequired();
            b.Property(x => x.RG).HasMaxLength(30);
            b.Property(x => x.Sexo).HasConversion<int>().IsRequired();
            b.Property(x => x.Nacionalidade).HasMaxLength(100);
            b.Property(x => x.Naturalidade).HasMaxLength(100);
            // Data de nascimento não precisa de fuso horário; mapear para 'date'
            b.Property(x => x.DataNascimento).HasColumnType("date");
            b.HasIndex(x => x.CPF).IsUnique();
            b.ToTable("Pessoa");
        });

        // Endereço (1:N com Pessoa)
        modelBuilder.Entity<Endereco>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.CEP).IsRequired().HasMaxLength(20);
            b.Property(x => x.Logradouro).IsRequired().HasMaxLength(200);
            b.Property(x => x.Numero).IsRequired().HasMaxLength(20);
            b.Property(x => x.Complemento).HasMaxLength(200);
            b.Property(x => x.Bairro).IsRequired().HasMaxLength(100);
            b.Property(x => x.Cidade).IsRequired().HasMaxLength(100);
            b.Property(x => x.Estado).IsRequired().HasMaxLength(100);
            b.Property(x => x.Pais).IsRequired().HasMaxLength(100);
            b.Property(x => x.Tipo).IsRequired().HasMaxLength(50);
            b.HasOne(e => e.Pessoa)
                .WithMany(p => p.Enderecos)
                .HasForeignKey(e => e.PessoaId)
                .OnDelete(DeleteBehavior.Cascade);
            b.ToTable("Endereco");
        });

        // Foto da Pessoa (1:1 com Pessoa)
        modelBuilder.Entity<FotoPessoa>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.ContentType).HasMaxLength(100);
            b.Property(x => x.StoragePath).IsRequired().HasMaxLength(300);
            b.HasOne(x => x.Pessoa)
                .WithOne(p => p.Foto)
                .HasForeignKey<FotoPessoa>(x => x.PessoaId)
                .OnDelete(DeleteBehavior.Cascade);
            b.ToTable("FotoPessoa");
        });

        // Aluno (composição)
        modelBuilder.Entity<Aluno>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Ativo).HasDefaultValue(true);
            b.Property(x => x.CertidaoNumero).HasMaxLength(100);
            b.Property(x => x.CertidaoTipo).HasConversion<int>();
            b.HasOne(a => a.Pessoa)
                .WithMany()
                .HasForeignKey(a => a.PessoaId)
                .OnDelete(DeleteBehavior.Cascade);
            b.ToTable("Aluno");
        });

        // Responsável do Aluno (tabela de junção Aluno <-> Pessoa)
        modelBuilder.Entity<ResponsavelAluno>(b =>
        {
            b.HasKey(x => new { x.AlunoId, x.ResponsavelId });
            b.HasOne(ra => ra.Aluno)
                .WithMany(a => a.ResponsavelAluno)
                .HasForeignKey(ra => ra.AlunoId)
                .OnDelete(DeleteBehavior.Cascade);
            b.HasOne(ra => ra.Responsavel)
                .WithMany()
                .HasForeignKey(ra => ra.ResponsavelId)
                .OnDelete(DeleteBehavior.Restrict);
            b.ToTable("ResponsavelAluno");
        });

        // Funcionário (composição) + relação 1:1 com User
        modelBuilder.Entity<Funcionario>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasOne(f => f.Pessoa)
                .WithMany()
                .HasForeignKey(f => f.PessoaId)
                .OnDelete(DeleteBehavior.Cascade);
            b.HasOne(f => f.Usuario)
                .WithOne()
                .HasForeignKey<Funcionario>(f => f.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
            b.HasIndex(f => f.UsuarioId).IsUnique();
            b.ToTable("Funcionario");
        });

        // Período
        modelBuilder.Entity<Periodo>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Nome).IsRequired().HasMaxLength(100);
            // Datas do período não precisam de fuso horário; mapear para 'date'
            b.Property(x => x.DataInicio).HasColumnType("date");
            b.Property(x => x.DataFim).HasColumnType("date");
            b.ToTable("Periodo");
        });

        // Curso e Currículo
        modelBuilder.Entity<Curso>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Nome).IsRequired();
            b.Property(x => x.Descricao).IsRequired();
            b.ToTable("Curso");
        });

        modelBuilder.Entity<Curriculo>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Nome).IsRequired();
            b.Property(x => x.Descricao).IsRequired();
            b.HasOne(c => c.Curso)
                .WithMany()
                .HasForeignKey(c => c.CursoId)
                .OnDelete(DeleteBehavior.Cascade);
            b.ToTable("Curriculo");
        });

        // Disciplina
        modelBuilder.Entity<Disciplina>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Nome).IsRequired();
            b.Property(x => x.Codigo).IsRequired();
            b.ToTable("Disciplina");
        });

        // Grade Curricular (PK composta: CurriculoId + DisciplinaId)
        modelBuilder.Entity<GradeCurricular>(b =>
        {
            b.HasKey(x => new { x.CurriculoId, x.DisciplinaId });
            b.HasOne(x => x.Curriculo)
                .WithMany(c => c.GradeCurricular)
                .HasForeignKey(x => x.CurriculoId)
                .OnDelete(DeleteBehavior.Cascade);
            b.HasOne(x => x.Disciplina)
                .WithMany()
                .HasForeignKey(x => x.DisciplinaId)
                .OnDelete(DeleteBehavior.Cascade);
            b.ToTable("GradeCurricular");
        });

        // Turma
        modelBuilder.Entity<Turma>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.CodigoTurma).IsRequired().HasMaxLength(50);
            b.Property(x => x.CapacidadeMaxima).IsRequired();
            b.HasOne(t => t.Periodo)
                .WithMany()
                .HasForeignKey(t => t.PeriodoId)
                .OnDelete(DeleteBehavior.Cascade);
            b.ToTable("Turma");
        });

        // TurmaDisciplina (PK: Id, índices únicos na composição TurmaId + DisciplinaId + FuncionarioId)
        modelBuilder.Entity<TurmaDisciplina>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasIndex(x => new { x.TurmaId, x.DisciplinaId, x.FuncionarioId }).IsUnique();
            b.HasOne(td => td.Turma)
                .WithMany()
                .HasForeignKey(td => td.TurmaId)
                .OnDelete(DeleteBehavior.Cascade);
            b.HasOne(td => td.Disciplina)
                .WithMany()
                .HasForeignKey(td => td.DisciplinaId)
                .OnDelete(DeleteBehavior.Cascade);
            b.HasOne(td => td.Funcionario)
                .WithMany()
                .HasForeignKey(td => td.FuncionarioId)
                .OnDelete(DeleteBehavior.Restrict);
            b.ToTable("TurmaDisciplina");
        });

        // Matrícula
        modelBuilder.Entity<Matricula>(b =>
        {
            b.HasKey(x => x.Id);
            // Data de matrícula como 'date' (sem fuso horário)
            b.Property(x => x.DataMatricula).HasColumnType("date");
            b.Property(x => x.DataMatricula).IsRequired();
            b.HasOne(m => m.Aluno)
                .WithMany()
                .HasForeignKey(m => m.AlunoId)
                .OnDelete(DeleteBehavior.Cascade);
            b.HasOne(m => m.Turma)
                .WithMany()
                .HasForeignKey(m => m.Turmaid)
                .OnDelete(DeleteBehavior.Cascade);
            b.ToTable("Matricula");
        });

        // Unidade
        modelBuilder.Entity<Unidade>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Numero).IsRequired();
            b.Property(x => x.Nome).IsRequired().HasMaxLength(200);
            b.Property(x => x.Peso).HasPrecision(10, 2);
            b.Property(x => x.NotaObtida).HasPrecision(10, 2);
            b.HasOne(u => u.Curriculo)
                .WithMany(c => c.Unidades)
                .HasForeignKey(u => u.CurriculoId)
                .OnDelete(DeleteBehavior.Cascade);
            b.ToTable("Unidade");
        });

        // Atividade Avaliativa
        modelBuilder.Entity<AtividadeAvaliativa>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Nome).IsRequired().HasMaxLength(200);
            b.Property(x => x.Peso).HasPrecision(10, 2);
            b.Property(x => x.NotaObtida).HasPrecision(10, 2);
            b.HasOne(a => a.Unidade)
                .WithMany()
                .HasForeignKey(a => a.UnidadeId)
                .OnDelete(DeleteBehavior.Cascade);
            b.ToTable("AtividadeAvaliativa");
        });

        // Nota por Atividade Avaliativa
        modelBuilder.Entity<NotaAtividadeAvaliativa>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Valor).HasPrecision(10, 2);
            b.HasOne(n => n.Matricula)
                .WithMany()
                .HasForeignKey(n => n.MatriculaId)
                .OnDelete(DeleteBehavior.Cascade);
            b.HasOne(n => n.Atividade)
                .WithMany()
                .HasForeignKey(n => n.AtividadeId)
                .OnDelete(DeleteBehavior.Restrict);
            b.ToTable("NotaAtividadeAvaliativa");
        });

        // Nota por Unidade
        modelBuilder.Entity<NotaUnidade>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Valor).HasPrecision(10, 2);
            b.HasOne(n => n.Matricula)
                .WithMany()
                .HasForeignKey(n => n.MatriculaId)
                .OnDelete(DeleteBehavior.Cascade);
            b.HasOne(n => n.Unidade)
                .WithMany()
                .HasForeignKey(n => n.UnidadeId)
                .OnDelete(DeleteBehavior.Restrict);
            b.ToTable("NotaUnidade");
        });

        // Histórico Escolar
        modelBuilder.Entity<HistoricoEscolar>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.SituacaoFinal).HasConversion<int>();
            b.Property(x => x.MediaFinal).HasPrecision(10, 2);
            b.HasOne(h => h.Matricula)
                .WithMany()
                .HasForeignKey(h => h.MatriculaId)
                .OnDelete(DeleteBehavior.Cascade);
            b.HasMany(h => h.Disciplinas)
                .WithOne(d => d.HistoricoEscolar)
                .HasForeignKey(d => d.HistoricoEscolarId)
                .OnDelete(DeleteBehavior.Cascade);
            b.ToTable("HistoricoEscolar");
        });

        // Histórico de Disciplina
        modelBuilder.Entity<HistoricoDisciplina>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.MediaFinal).HasPrecision(10, 2);
            b.Property(x => x.Frequencia).HasPrecision(5, 2);
            b.Property(x => x.Observacao).HasMaxLength(500);
            b.HasOne(hd => hd.Disciplina)
                .WithMany()
                .HasForeignKey(hd => hd.DisciplinaId)
                .OnDelete(DeleteBehavior.Cascade);
            b.ToTable("HistoricoDisciplina");
        });

        base.OnModelCreating(modelBuilder);
    }
}
