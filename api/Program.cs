using System.Text;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RappyFastFood.API.Data;
using RappyFastFood.API.Repositories;
using RappyFastFood.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opcoes =>
    opcoes.UseSqlServer(builder.Configuration.GetConnectionString("Padrao")));

builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<ProdutoRepository>();
builder.Services.AddScoped<PedidoRepository>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ProdutoService>();
builder.Services.AddScoped<PedidoService>();

var chave = builder.Configuration["Jwt:Chave"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opcoes =>
    {
        opcoes.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chave))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddCors(opcoes =>
    opcoes.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddControllers();

var app = builder.Build();

app.UseExceptionHandler(erro => erro.Run(async contexto =>
{
    var excecao = contexto.Features.Get<IExceptionHandlerFeature>()?.Error;
    contexto.Response.StatusCode = excecao is InvalidOperationException ? 400 : 500;
    contexto.Response.ContentType = "application/json";
    var mensagem = excecao is InvalidOperationException ? excecao.Message : "Erro interno.";
    await contexto.Response.WriteAsJsonAsync(new { erro = mensagem });
}));

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var escopo = app.Services.CreateScope())
{
    await DbSeeder.PopularAsync(escopo.ServiceProvider.GetRequiredService<AppDbContext>());
}

app.Run();
