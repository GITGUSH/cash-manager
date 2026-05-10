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
            var lista = service.Listar(idUsuario);
            return Results.Ok(lista);
        }).RequireAuthorization();

        app.MapDelete("/conta/{id}", (int id, HttpContext http) =>
{
            var idUsuario = int.Parse(http.User.FindFirst("id")!.Value);
            var deletou = service.Deletar(id, idUsuario);
        
            if (!deletou)
                return Results.BadRequest("Conta possui operações vinculadas e não pode ser deletada.");
        
            return Results.Ok("Conta deletada com sucesso!");
        }).RequireAuthorization();
    }
}