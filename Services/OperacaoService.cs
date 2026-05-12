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
        using var cmd = new NpgsqlCommand(@"
        SELECT o.id_operacao, o.descricao, o.valor, o.tipo_es, o.data_operacao, o.id_conta, o.id_categoria, c.nome
        FROM operacao o
        INNER JOIN conta c ON o.id_conta = c.id_conta
        WHERE o.id_usuario = @idUsuario
        ORDER BY o.data_operacao DESC", conn);
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
                IdCategoria  = reader.GetInt32(6),
                NomeConta    = reader.GetString(7)
            });
        }

        return operacoes;
    }

    public void Deletar(int idOperacao, int idUsuario)
{
    using var conn = Conexao.Abrir();

    // Busca o valor e tipo da operação antes de deletar
    decimal valor = 0;
    string tipoES = "";
    int idConta = 0;

    using (var cmdBusca = new NpgsqlCommand("SELECT valor, tipo_es, id_conta FROM operacao WHERE id_operacao = @idOperacao AND id_usuario = @idUsuario", conn))
    {
        cmdBusca.Parameters.AddWithValue("@idOperacao", idOperacao);
        cmdBusca.Parameters.AddWithValue("@idUsuario", idUsuario);

        using var reader = cmdBusca.ExecuteReader();
        if (reader.Read())
        {
            valor   = reader.GetDecimal(0);
            tipoES  = reader.GetString(1);
            idConta = reader.GetInt32(2);
        }
    }

    // Reverte o saldo da conta
    string operacaoSaldo = tipoES == "E" ? "-" : "+";
    using (var cmdSaldo = new NpgsqlCommand($"UPDATE conta SET saldo = saldo {operacaoSaldo} @valor WHERE id_conta = @idConta", conn))
    {
        cmdSaldo.Parameters.AddWithValue("@valor", valor);
        cmdSaldo.Parameters.AddWithValue("@idConta", idConta);
        cmdSaldo.ExecuteNonQuery();
    }

    // Deleta a operação
    using var cmdDelete = new NpgsqlCommand("DELETE FROM operacao WHERE id_operacao = @idOperacao", conn);
    cmdDelete.Parameters.AddWithValue("@idOperacao", idOperacao);
    cmdDelete.ExecuteNonQuery();
}
}