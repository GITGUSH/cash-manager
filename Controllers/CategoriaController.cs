public static class CategoriaController
{
    public static void MapearRotas(WebApplication app)
    {
        var service = new CategoriaService();

        app.MapPost("/categoria", (Categoria categoria, HttpContext http) =>
{
            var idUsuario = int.Parse(http.User.FindFirst("id")!.Value);
            service.Inserir(categoria.Nome!, categoria.TipoES!, idUsuario);
            return Results.Ok("Categoria criada com sucesso!");
        }).RequireAuthorization();

        app.MapGet("/categorias", (HttpContext http) =>
        {
            var idUsuario = int.Parse(http.User.FindFirst("id")!.Value);
            var lista = service.Listar(idUsuario);
            return Results.Ok(lista);
        }).RequireAuthorization();

        app.MapDelete("/categoria/{id}", (int id, HttpContext http) =>
        {
            var idUsuario = int.Parse(http.User.FindFirst("id")!.Value);
            var deletou = service.Deletar(id, idUsuario);
        
            if (!deletou)
                return Results.BadRequest("Categoria possui operações vinculadas e não pode ser deletada.");
        
            return Results.Ok("Categoria deletada com sucesso!");
        }).RequireAuthorization();
    }
}
