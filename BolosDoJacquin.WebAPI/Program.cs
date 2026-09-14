using BolosDoJacquin.WebAPI.BdContext;
using BolosDoJacquin.WebAPI.Interfaces;
using BolosDoJacquin.WebAPI.Repositories;
using BolosDoJacquin.WebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configuração do EF Core - Banco de Dados
builder.Services.AddDbContext<JacquinContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()
    ));

// Registra os controllers e evita loop nos relacionamentos JSON
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Injeção de dependência das nossas fatias
builder.Services.AddScoped<ITipoUsuario, TipoUsuarioRepository>();
builder.Services.AddScoped<ICategoria, CategoriaRepository>();
builder.Services.AddScoped<IUsuario, UsuarioRepository>();
builder.Services.AddScoped<IProduto, ProdutoRepository>();
builder.Services.AddScoped<IAvaliacao, AvaliacaoRepository>();
builder.Services.AddScoped<SightengineModerationService>();
builder.Services.AddScoped<CloudinaryService>();

// Autenticação JWT
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
        ValidIssuer = "BolosDoJacquin.WebAPI",
        ValidateAudience = true,
        ValidAudience = "BolosDoJacquin.WebAPI",
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(5),
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        )
    };
});

// Registra o serviço de autorização
// Registra o serviço de autorização
// Registra o serviço de autorização
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();