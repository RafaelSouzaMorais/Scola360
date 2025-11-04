using Scola360.Academico.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Scola360.Academico.Domain.Interfaces;
using Scola360.Academico.Infrastructure.Repositories;
using Scola360.Academico.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Scola360.Academico.Application.Interfaces;
using Scola360.Academico.Infrastructure.Auth;
using Scola360.Academico.Domain.Entities;
using System.Security.Cryptography;
using AutoMapper;
using Scola360.Academico.Application.Profiles;
using DotNetEnv;
using System.Text.RegularExpressions;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

try { Env.Load(); } catch { }

// Permite comportamento legado de timestamp para que DateTime com Kind=Unspecified não aplique conversões indesejadas
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var configuration = builder.Configuration;

// CORS: origens configuráveis via env Cors__Origins (separadas por vírgula) ou appsettings Cors:Origins
var corsOrigins = Environment.GetEnvironmentVariable("Cors__Origins")
                 ?? configuration["Cors:Origins"]
                 ?? "http://localhost:5173";
var allowedOrigins = corsOrigins.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", p =>
        p.WithOrigins(allowedOrigins)
         .AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials());
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var cs = BuildNpgsqlConnectionString(configuration);
    options.UseNpgsql(cs);
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenProvider, JwtTokenProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Alunos DI
builder.Services.AddScoped<IAlunoRepository, AlunoRepository>();
builder.Services.AddScoped<IAlunoService, AlunoService>();

// Funcionarios DI
builder.Services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
builder.Services.AddScoped<IFuncionarioService, FuncionarioService>();

// Endereco DI
builder.Services.AddScoped<IEnderecoRepository, EnderecoRepository>();
builder.Services.AddScoped<IEnderecoService, EnderecoService>();

// Pessoas DI
builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();
builder.Services.AddScoped<IPessoaService, PessoaService>();

// ResponsavelAluno DI
builder.Services.AddScoped<IResponsavelAlunoRepository, ResponsavelAlunoRepository>();

// Disciplina DI
builder.Services.AddScoped<IDisciplinaRepository, DisciplinaRepository>();
builder.Services.AddScoped<IDisciplinaService, DisciplinaService>();

//Curso DI
builder.Services.AddScoped<ICursoRepository, CursoRepository>();
builder.Services.AddScoped<ICursoService, CursoService>();

// Periodo DI
builder.Services.AddScoped<IPeriodoRepository, PeriodoRepository>();
builder.Services.AddScoped<IPeriodoService, PeriodoService>();

// Curriculo + GradeCurricular DI
builder.Services.AddScoped<ICurriculoRepository, CurriculoRepository>();
builder.Services.AddScoped<IGradeCurricularRepository, GradeCurricularRepository>();
builder.Services.AddScoped<ICurriculoService, CurriculoService>();
builder.Services.AddScoped<IGradeCurricularService, GradeCurricularService>();

builder.Services.AddAutoMapper(typeof(AlunoProfile).Assembly,
                               typeof(PessoaProfile).Assembly,
                               typeof(DisciplinaProfile).Assembly,
                               typeof(CursoProfile).Assembly,
                               typeof(PeriodoProfile).Assembly,
                               typeof(CurriculoProfile).Assembly,
                               typeof(FuncionarioProfile).Assembly);

// Configure JWT Auth
var jwtKey = configuration["Jwt:Key"] ?? Environment.GetEnvironmentVariable("Jwt__Key") ?? "dev-secret-change-me-32-bytes-minimum";
var issuer = configuration["Jwt:Issuer"] ?? Environment.GetEnvironmentVariable("Jwt__Issuer") ?? "SistemaAcademico";
var audience = configuration["Jwt:Audience"] ?? Environment.GetEnvironmentVariable("Jwt__Audience") ?? "SistemaAcademicoAudience";
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        RequireExpirationTime = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ClockSkew = TimeSpan.FromMinutes(1)
    };
});

var app = builder.Build();

// Apply EF Core migrations only (avoid EnsureCreated to prevent schema drift)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    // Seed roles
    var defaultRoles = new[] { "admin", "teacher", "secretary", "financial" };
    foreach (var r in defaultRoles)
    {
        if (!db.Roles.Any(x => x.Name == r))
            db.Roles.Add(new Role { Name = r });
    }
    db.SaveChanges();

    // Seed admin user
    if (!db.Users.Include(u => u.Roles).Any())
    {
        var password = "admin123";
        var salt = RandomNumberGenerator.GetBytes(32);
        using var hmac = new HMACSHA256(salt);
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        var adminRole = db.Roles.First(x => x.Name == "admin");

        var user = new User
        {
            Username = "admin",
            PasswordSalt = Convert.ToBase64String(salt),
            PasswordHash = Convert.ToBase64String(hash),
            Active = true,
            Roles = new List<Role> { adminRole }
        };

        db.Users.Add(user);
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// In containers we usually run HTTP only in Development
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

// ---- helpers ----
static string BuildNpgsqlConnectionString(IConfiguration configuration)
{
    // 1. Tenta obter uma URL de conexão completa (padrão em muitas PaaS)
    var connectionUrl = Environment.GetEnvironmentVariable("CONN_STRING") ??
                        Environment.GetEnvironmentVariable("POSTGRES_URL") ??
                        Environment.GetEnvironmentVariable("POSTGRESQL_URL");

    if (Uri.TryCreate(connectionUrl, UriKind.Absolute, out var uri))
    {
        var userInfo = uri.UserInfo.Split(':', 2);
        var cs = new Npgsql.NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.IsDefaultPort ? 5432 : uri.Port,
            Username = userInfo[0],
            Password = userInfo.Length > 1 ? userInfo[1] : string.Empty,
            Database = uri.AbsolutePath.Trim('/'),
        };
        Console.WriteLine(cs);
        // Adiciona SslMode se estiver na query string da URL
        var queryParams = System.Web.HttpUtility.ParseQueryString(uri.Query);
        if (queryParams.AllKeys.Contains("sslmode", StringComparer.OrdinalIgnoreCase))
        {
            var sslMode = queryParams["sslmode"];
            if (Enum.TryParse<SslMode>(sslMode, ignoreCase: true, out var mode))
            {
                cs.SslMode = mode;
            }
        }
        return cs.ToString();
    }

    // 2. Se não for URL, tenta obter a string de conexão padrão (appsettings ou env)
    var connectionString = configuration.GetConnectionString("Default") ??
                           configuration.GetConnectionString("DefaultConnection");

    if (!string.IsNullOrWhiteSpace(connectionString))
    {
        return NormalizeConnectionString(connectionString);
    }

    // 3. Fallback para variáveis de ambiente individuais (padrão Docker/PG)
    var inContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
    var csBuilder = new Npgsql.NpgsqlConnectionStringBuilder
    {
        Host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? Environment.GetEnvironmentVariable("PGHOST") ?? (inContainer ? "host.docker.internal" : "localhost"),
        Port = int.TryParse(Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? Environment.GetEnvironmentVariable("PGPORT"), out var port) ? port : 5432,
        Database = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? Environment.GetEnvironmentVariable("PGDATABASE") ?? "SistemaAcademicoDb",
        Username = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? Environment.GetEnvironmentVariable("PGUSER") ?? "postgres",
        Password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? Environment.GetEnvironmentVariable("PGPASSWORD") ?? "postgres"
    };

    return NormalizeConnectionString(csBuilder.ToString());
}

static string NormalizeConnectionString(string connectionString)
{
    try
    {
        var builder = new Npgsql.NpgsqlConnectionStringBuilder(connectionString);

        // Corrige o problema do "tcp://" que algumas plataformas injetam
        if (builder.Host != null && builder.Host.StartsWith("tcp://", StringComparison.OrdinalIgnoreCase))
        {
            var host = builder.Host["tcp://".Length..];
            var parts = host.Split(':', 2);
            builder.Host = parts[0];
            if (parts.Length > 1 && int.TryParse(parts[1], out var port))
            {
                builder.Port = port;
            }
        }
        return builder.ToString();
    }
    catch
    {
        // Se o NpgsqlConnectionStringBuilder falhar, retorna a string original
        // A lógica anterior de Regex pode ser adicionada aqui como um fallback mais robusto se necessário
        return connectionString;
    }
}
