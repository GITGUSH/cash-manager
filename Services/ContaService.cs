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

    public List<Conta> Listar()
    {
        var contas = new List<Conta>();

        using var conn = Conexao.Abrir();
        using var cmd = new NpgsqlCommand("SELECT id_conta, nome, id_usuario, saldo, data_inclusao FROM conta", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
            {
                contas.Add(new Conta
                {
                    IdConta = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                    Saldo = reader.GetDecimal(3),
                    DataInclusao = reader.GetDateTime(4)
                });
            }
        return contas;
    }
}
