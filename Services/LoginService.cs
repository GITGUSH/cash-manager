using Npgsql;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class LoginService
{
    private Usuario? BuscarPorEmail(string email)
    {
        using var conn = Conexao.Abrir();
        using var cmd = new NpgsqlCommand("SELECT id_usuario, nome, email, senha_hash FROM usuario WHERE email = @email", conn);
        cmd.Parameters.AddWithValue("@email", email);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Usuario
            {
                IdUsuario  = reader.GetInt32(0),
                Nome       = reader.GetString(1),
                Email      = reader.GetString(2),
                SenhaHash  = reader.GetString(3)
            };   
        }
        return null;
    }

    public string? Login(string email, string senha)
    {
        var usuario = BuscarPorEmail(email); 

        if (usuario == null) return null; //Usuário não encontrado

        if (!BCrypt.Net.BCrypt.Verify(senha, usuario.SenhaHash)) return null; //Senha incorreta

        return GeraToken(usuario); //Gera o token
    }

    private string GeraToken(Usuario usuario)
    {
        var secret = Environment.GetEnvironmentVariable("JWT_SECRET")!;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[] //Dados que ficam dentro do token
        {
            new Claim("id", usuario.IdUsuario.ToString()!),
            new Claim("nome", usuario.Nome!),
            new Claim("email", usuario.Email!)
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8), //8 horas para expirar a sessão
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}