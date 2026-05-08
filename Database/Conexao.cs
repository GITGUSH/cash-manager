using Npgsql;
using DotNetEnv;

public class Conexao //Cria a conexão com o banco de dados, buscando as credenciais no arquivo de ambiente
{
    public static NpgsqlConnection Abrir()
    {
        Env.Load();

        string connString = $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
                            $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                            $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
                            $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
                            $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")}";

        var conn = new NpgsqlConnection(connString);
        conn.Open();
        return conn;
    }
}