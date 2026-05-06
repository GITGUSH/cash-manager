using Npgsql;
using BCrypt.Net;
public class UsuarioService
{
    public void Inserir(string nome, string email, string senha)
    {
        string senhaHash = BCrypt.Net.BCrypt.HashPassword(senha);
        using var conn = Conexao.Abrir();
        using var cmd = new NpgsqlCommand("CALL inserir_usuario(@nome, @email, @senha)", conn);

        cmd.Parameters.AddWithValue("@nome", nome);
        cmd.Parameters.AddWithValue("@email", email);
        cmd.Parameters.AddWithValue("@senha", senhaHash);

        cmd.ExecuteNonQuery();
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