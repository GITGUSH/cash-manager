public static class ContaController
{
    public static void MapearRotas(WebApplication app)
    {
        var service = new ContaService();

        app.MapPost("/conta", (Conta conta, HttpContext http) =>
        {
            var idUsuario = int.Parse(http.User.FindFirst("id")!.Value);
            service.Inserir(conta.Nome!, idUsuario, conta.Saldo);
            return Results.Ok("Conta criada com sucesso!");
        }).RequireAuthorization();

        app.MapGet("/contas", (HttpContext http) =>
        {
            var idUsuario = int.Parse(http.User.FindFirst("id")!.Value);
            var lista = service.Listar();
            return Results.Ok(lista);
        }).RequireAuthorization();
    }
}