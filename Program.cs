using BolosDoJacquin.Applications.Services;
using BolosDoJacquin.BdContextBolos;
using BolosDoJacquin.Domain.Interfaces;
using BolosDoJacquin.Infra.Cloudinary;
using BolosDoJacquin.Infra.Security;
using BolosDoJacquin.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Insira um token válido para ter acesso aos endpoint da API"
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

// 1. Lendo a String de Conexão do appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BolosContext>(options => options.UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure()));

builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IAvaliacaoRepository, AvaliacaoRepository>();
builder.Services.AddScoped<ISenhaCriptografia, BCryptSenhaCriptografia>();
builder.Services.AddScoped<IUploadImagemService, CloudinaryService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<ProdutoService>();
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<AvaliacaoService>();

builder.Services.AddHttpClient<BolosDoJacquin.Domain.Interfaces.IModeracaoTextoService, BolosDoJacquin.Infra.Sightengine.ModeracaoTextoService>(client =>
{
    client.BaseAddress = new Uri("https://api.sightengine.com/1.0/");
});

var key = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JwtSecret")!);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Desligados porque o TokenService não os envia ainda
        ValidateIssuer = false,
        ValidateAudience = false,
        // Valida se o token ainda esta dentro do prazo
        ValidateLifetime = true,
        // Define a tolerancia de clock entre servidores
        ClockSkew = TimeSpan.FromMinutes(5),
        // Usa a SUA variável 'key' que já estava declarada ali em cima!
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

app.UseMiddleware<BolosDoJacquin.Middlewares.ErrorMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();