using Npgsql;
using BCrypt.Net;
public class UsuarioService
{
    public bool Inserir(string nome, string email, string senha)
    {
        using var conn = Conexao.Abrir();
    
        // Verifica se email já existe
        using var cmdVerifica = new NpgsqlCommand("SELECT COUNT(*) FROM usuario WHERE email = @email", conn);
        cmdVerifica.Parameters.AddWithValue("@email", email);
        var total = (long)cmdVerifica.ExecuteScalar()!;
    
        if (total > 0) return false; // email já cadastrado
    
        string senhaHash = BCrypt.Net.BCrypt.HashPassword(senha);
    
        using var cmd = new NpgsqlCommand("CALL inserir_usuario(@nome, @email, @senha)", conn);
        cmd.Parameters.AddWithValue("@nome", nome);
        cmd.Parameters.AddWithValue("@email", email);
        cmd.Parameters.AddWithValue("@senha", senhaHash);
        cmd.ExecuteNonQuery();
    
        return true;
    }

    public List<Usuario> Listar()
    {
        var usuarios = new List<Usuario>();

        using var conn = Conexao.Abrir();
        using var cmd = new NpgsqlCommand("SELECT id_usuario, nome, email, data_inclusao FROM usuario", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            usuarios.Add(new Usuario
            {
                IdUsuario    = reader.GetInt32(0),
                Nome         = reader.GetString(1),
                Email        = reader.GetString(2),
                DataInclusao = reader.GetDateTime(3)
            });
        }

        return usuarios;
    }
}