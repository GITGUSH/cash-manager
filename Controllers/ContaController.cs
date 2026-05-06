public static class ContaController
{
    public static void MapearRotas(WebApplication app)
    {
        var service = new ContaService();

        app.MapPost("/conta", (Conta conta) =>
        {
            service.Inserir(conta.Nome!, conta.IdUsuario, conta.Saldo);
            return Results.Ok("Conta criada com sucesso!");
        });

        app.MapGet("/contas", () =>
        {
            var lista = service.Listar();
            return Results.Ok(lista);
        });
    }
}