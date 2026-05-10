using Npgsql;

public class ContaService
{
    public void Inserir(string nome, int idUsuario, decimal saldo)
    {
        using var conn = Conexao.Abrir();
        using var cmd = new NpgsqlCommand("CALL inserir_conta(@nome, @idUsuario, @saldo)", conn);

        cmd.Parameters.AddWithValue("@nome", nome);
        cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
        cmd.Parameters.AddWithValue("@saldo", saldo);

        cmd.ExecuteNonQuery();
    }

        public List<Conta> Listar(int idUsuario)
    {
        var contas = new List<Conta>();
    
        using var conn = Conexao.Abrir();
        using var cmd = new NpgsqlCommand("SELECT id_conta, nome, saldo FROM conta WHERE id_usuario = @idUsuario", conn);
        cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
        using var reader = cmd.ExecuteReader();
    
        while (reader.Read())
        {
            contas.Add(new Conta
            {
                IdConta = reader.GetInt32(0),
                Nome    = reader.GetString(1),
                Saldo   = reader.GetDecimal(2)
            });
        }
    
        return contas;
    }

    public bool Deletar(int idConta, int idUsuario)
    {
        using var conn = Conexao.Abrir();
    
        // Verifica se tem operações vinculadas
        using var cmdVerifica = new NpgsqlCommand("SELECT COUNT(*) FROM operacao WHERE id_conta = @idConta", conn);
        cmdVerifica.Parameters.AddWithValue("@idConta", idConta);
        var total = (long)cmdVerifica.ExecuteScalar()!;
    
        if (total > 0) return false; // tem operações, não deleta
    
        using var cmd = new NpgsqlCommand("DELETE FROM conta WHERE id_conta = @idConta AND id_usuario = @idUsuario", conn);
        cmd.Parameters.AddWithValue("@idConta", idConta);
        cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
        cmd.ExecuteNonQuery();
    
        return true; // deletou com sucesso
    }
}
