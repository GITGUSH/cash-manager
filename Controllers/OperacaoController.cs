public static class OperacaoController
{
    public static void MapearRotas(WebApplication app)
    {
        var service = new OperacaoService();

        app.MapPost("/operacao", (Operacao operacao, HttpContext http) =>
{
            var idUsuario = int.Parse(http.User.FindFirst("id")!.Value);
            service.Inserir(operacao.Descricao!, operacao.Valor, operacao.TipoES!, operacao.IdConta, idUsuario, operacao.IdCategoria);
            return Results.Ok("Operação criada com sucesso!");
        }).RequireAuthorization();

        app.MapGet("/operacoes", (HttpContext http) =>
        {
            var idUsuario = int.Parse(http.User.FindFirst("id")!.Value);
            var lista = service.Listar(idUsuario);
            return Results.Ok(lista);
        }).RequireAuthorization();

        app.MapDelete("/operacao/{id}", (int id, HttpContext http) =>
        {
            var idUsuario = int.Parse(http.User.FindFirst("id")!.Value);
            service.Deletar(id, idUsuario);
            return Results.Ok("Operação deletada com sucesso!");
        }).RequireAuthorization();
    }
}