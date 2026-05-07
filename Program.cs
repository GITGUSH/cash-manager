using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

//Configuração do JWT
var secret = Environment.GetEnvironmentVariable("JWT_SECRET")!;
var key = Encoding.UTF8.GetBytes(secret);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
           options.TokenValidationParameters = new TokenValidationParameters
           {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false   
           };
        });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

UsuarioController.MapearRotas(app);
ContaController.MapearRotas(app);
CategoriaController.MapearRotas(app);
OperacaoController.MapearRotas(app);
LoginController.MapearRotas(app);

app.Run();
