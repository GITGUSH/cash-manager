public static class OperacaoController
{
    public static void MapearRotas(WebApplication app)
    {
        var service = new OperacaoService();

        app.MapPost("/operacao", (Operacao operacao) =>
        {
           service.Inserir(operacao.Descricao!, operacao.Valor, operacao.TipoES!, operacao.IdConta, operacao.IdUsuario, operacao.IdCategoria); 
           return Results.Ok("Operação realizada com sucesso!");
        });

        app.MapGet("/operacoes", () =>
        {
            var lista = service.Listar();
            return Results.Ok(lista);
        });
    }
}