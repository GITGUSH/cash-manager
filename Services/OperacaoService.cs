using Npgsql;

public class OperacaoService
{
    public void Inserir(string descricao, decimal valor, string tipoES, int idConta, int idUsuario, int idCategoria)
    {
        using var conn = Conexao.Abrir();
        using var cmd = new NpgsqlCommand("CALL inserir_operacao(@descricao, @valor, @tipoES, @idConta, @idUsuario, @idCategoria)", conn);

        cmd.Parameters.AddWithValue("@descricao", descricao);
        cmd.Parameters.AddWithValue("@valor", valor);
        cmd.Parameters.AddWithValue("@tipoES", tipoES);
        cmd.Parameters.AddWithValue("@idConta", idConta);
        cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
        cmd.Parameters.AddWithValue("@idCategoria", idCategoria);

        cmd.ExecuteNonQuery();
        
    }

        public List<Operacao> Listar(int idUsuario)
    {
        var operacoes = new List<Operacao>();

        using var conn = Conexao.Abrir();
        using var cmd = new NpgsqlCommand("SELECT id_operacao, descricao, valor, tipo_es, data_operacao, id_conta, id_categoria FROM operacao WHERE id_usuario = @idUsuario", conn);
        cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            operacoes.Add(new Operacao
            {
                IdOperacao   = reader.GetInt32(0),
                Descricao    = reader.GetString(1),
                Valor        = reader.GetDecimal(2),
                TipoES       = reader.GetString(3),
                DataOperacao = reader.GetDateTime(4),
                IdConta      = reader.GetInt32(5),
                IdCategoria  = reader.GetInt32(6)
            });
        }

        return operacoes;
    }
}