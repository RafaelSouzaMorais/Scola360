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
    var inContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
    var defaultHost = inContainer ? "host.docker.internal" : "localhost";

    // Prioriza variáveis de ambiente; depois appsettings; por fim fallback
    var cs = Environment.GetEnvironmentVariable("ConnectionStrings__Default")
             ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
             ?? configuration.GetConnectionString("Default")
             ?? configuration.GetConnectionString("DefaultConnection")
             ?? $"Host={defaultHost};Port=5432;Database=SistemaAcademicoDb;Username=postgres;Password=postgres";

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
