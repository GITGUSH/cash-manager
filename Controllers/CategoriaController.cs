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

        app.MapGet("/categorias", () =>
        {
            var lista = service.Listar();
            return Results.Ok(lista);
        }).RequireAuthorization();
    }
}
