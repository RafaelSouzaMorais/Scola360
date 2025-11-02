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

var builder = WebApplication.CreateBuilder(args);

try { Env.Load(); } catch { }

// // Permite comportamento legado de timestamp para que DateTime com Kind=Unspe
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
                               typeof(CurriculoProfile).Assembly);

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
    // 1) Prioriza variáveis de ambiente comuns em PaaS
    string? raw =
        Environment.GetEnvironmentVariable("ConnectionStrings__Default") ??
        Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") ??
        Environment.GetEnvironmentVariable("DATABASE_URL") ??
        Environment.GetEnvironmentVariable("POSTGRES_URL") ??
        Environment.GetEnvironmentVariable("POSTGRESQL_URL") ??
        configuration.GetConnectionString("Default") ??
        configuration.GetConnectionString("DefaultConnection");

    // Fallback quando nada foi informado
    var inContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
    var defaultHost = inContainer ? "host.docker.internal" : "localhost";

    if (string.IsNullOrWhiteSpace(raw))
    {
        var host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? defaultHost;
        var port = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432";
        var db = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "SistemaAcademicoDb";
        var user = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "postgres";
        var pwd = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "postgres";
        return $"Host={host};Port={port};Database={db};Username={user};Password={pwd}";
    }

    // Se for URL (postgres:// ou postgresql://)
    if (TryFromPostgresUrl(raw, out var csFromUrl))
        return csFromUrl!;

    // Se vier no formato incorreto com tcp:// no Host/Server, normaliza
    raw = NormalizeTcpInHostOrServer(raw);

    // Se mesmo assim não houver Host=, tente montar a partir de variáveis separadas
    if (!raw.Contains("Host=", StringComparison.OrdinalIgnoreCase))
    {
        var host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? defaultHost;
        var port = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432";
        raw = $"Host={host};Port={port};" + raw.Trim(';');
    }

    return raw;
}

static bool TryFromPostgresUrl(string value, out string? connectionString)
{
    connectionString = null;
    if (value.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase) ||
        value.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase))
    {
        if (!Uri.TryCreate(value, UriKind.Absolute, out var uri))
            return false;

        var host = uri.Host;
        var port = uri.IsDefaultPort ? 5432 : uri.Port;
        var db = Uri.UnescapeDataString(uri.AbsolutePath.Trim('/'));

        var user = string.Empty;
        var pass = string.Empty;
        if (!string.IsNullOrEmpty(uri.UserInfo))
        {
            var parts = uri.UserInfo.Split(':', 2);
            user = Uri.UnescapeDataString(parts[0]);
            if (parts.Length > 1)
                pass = Uri.UnescapeDataString(parts[1]);
        }

        // Trata sslmode da querystring se existir
        var sslMode = ExtractQuery(uri.Query, "sslmode");
        var sslPart = string.IsNullOrEmpty(sslMode) ? string.Empty : $";Ssl Mode={sslMode}";

        connectionString = $"Host={host};Port={port};Database={db};Username={user};Password={pass}{sslPart}";
        return true;
    }

    return false;
}

static string NormalizeTcpInHostOrServer(string value)
{
    // Normaliza Host=tcp://... e Server=tcp://...
    foreach (var key in new[] { "Host", "Server", "Data Source" })
    {
        var pattern = $@"{Regex.Escape(key)}\s*=\s*tcp://([^;]+)";
        var match = Regex.Match(value, pattern, RegexOptions.IgnoreCase);
        if (match.Success)
        {
            var hostPort = match.Groups[1].Value;
            var host = hostPort;
            var port = "";
            var colon = hostPort.LastIndexOf(':');
            if (colon > -1)
            {
                host = hostPort[..colon];
                port = hostPort[(colon + 1)..];
            }

            var replaced = Regex.Replace(value, pattern, $"{key}={host}", RegexOptions.IgnoreCase);
            if (!string.IsNullOrEmpty(port))
            {
                // Se já existir Port=, não duplica
                if (!Regex.IsMatch(replaced, @";\s*Port\s*=", RegexOptions.IgnoreCase))
                {
                    // Inserir Port logo após o par Host/Server
                    replaced = Regex.Replace(replaced, $@"{Regex.Escape(key)}\s*=[^;]+", m => m.Value + $";Port={port}", RegexOptions.IgnoreCase);
                }
            }
            return replaced;
        }
    }

    // Caso a string inteira seja tcp://host:port, tentar compor com variáveis padrão
    if (value.StartsWith("tcp://", StringComparison.OrdinalIgnoreCase))
    {
        if (Uri.TryCreate(value, UriKind.Absolute, out var uri))
        {
            var host = uri.Host;
            var port = uri.IsDefaultPort ? 5432 : uri.Port;
            var db = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "SistemaAcademicoDb";
            var user = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "postgres";
            var pwd = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "postgres";
            return $"Host={host};Port={port};Database={db};Username={user};Password={pwd}";
        }
    }

    return value;
}

static string? ExtractQuery(string query, string key)
{
    if (string.IsNullOrEmpty(query)) return null;
    var q = query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    foreach (var kv in q)
    {
        var parts = kv.Split('=', 2);
        if (parts.Length == 2 && parts[0].Equals(key, StringComparison.OrdinalIgnoreCase))
            return Uri.UnescapeDataString(parts[1]);
    }
    return null;
}
