using Npgsql;

public class CategoriaService
{
    public void Inserir(string nome, string tipoES, int idUsuario) //Função para inserir uma categoria
    {
        using var conn = Conexao.Abrir();
        using var cmd = new NpgsqlCommand("CALL inserir_categoria(@nome, @tipoES, @idUsuario)", conn);

        cmd.Parameters.AddWithValue("@nome", nome);
        cmd.Parameters.AddWithValue("@tipoES", tipoES);
        cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

        cmd.ExecuteNonQuery();
    }

   public List<Categoria> Listar(int idUsuario)
    {
        var categorias = new List<Categoria>();
    
        using var conn = Conexao.Abrir();
        using var cmd = new NpgsqlCommand("SELECT id_categoria, nome, tipo_es FROM categoria WHERE id_usuario = @idUsuario",    conn);
        cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
        using var reader = cmd.ExecuteReader();
    
        while (reader.Read())
        {
            categorias.Add(new Categoria
            {
                IdCategoria = reader.GetInt32(0),
                Nome        = reader.GetString(1),
                TipoES      = reader.GetString(2)
            });
        }
    
        return categorias;
    }

    public void Deletar(int idCategoria, int idUsuario)
    {
        using var conn = Conexao.Abrir();
        using var cmd = new NpgsqlCommand("DELETE FROM categoria WHERE id_categoria = @idCategoria AND id_usuario = @idUsuario", conn);
        cmd.Parameters.AddWithValue("@idCategoria", idCategoria);
        cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
        cmd.ExecuteNonQuery();
    }
}