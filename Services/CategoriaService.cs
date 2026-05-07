using Npgsql;

public class CategoriaService
{
    public void Inserir(string nome, string tipoES, int idUsuario)
    {
        using var conn = Conexao.Abrir();
        using var cmd = new NpgsqlCommand("CALL inserir_categoria(@nome, @tipoES, @idUsuario)", conn);

        cmd.Parameters.AddWithValue("@nome", nome);
        cmd.Parameters.AddWithValue("@tipoES", tipoES);
        cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

        cmd.ExecuteNonQuery();
    }

    public List<Categoria> Listar()
    {
        var categorias = new List<Categoria>();

        using var conn = Conexao.Abrir();
        using var cmd = new NpgsqlCommand("SELECT id_categoria, nome, tipo_es, id_usuario FROM categoria", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            categorias.Add(new Categoria
            {
                IdCategoria = reader.GetInt32(0),
                Nome = reader.GetString(1),
                TipoES = reader.GetString(2),
                IdUsuario = reader.GetInt32(3)
            });
        }
        
        return categorias;
    }
}