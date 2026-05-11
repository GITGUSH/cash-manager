using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

//Configuração do JWT
var secret = Environment.GetEnvironmentVariable("JWT_SECRET")!;
var key = Encoding.UTF8.GetBytes(secret);

/*builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = null;
});*/

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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "http://127.0.0.1:5500",
                "http://localhost:5500"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.MapPost("/shutdown", (IHostApplicationLifetime lifetime) =>
{
    lifetime.StopApplication();
    return Results.Ok("Sistema encerrado.");
});

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

UsuarioController.MapearRotas(app);
ContaController.MapearRotas(app);
CategoriaController.MapearRotas(app);
OperacaoController.MapearRotas(app);
LoginController.MapearRotas(app);

app.Run();
