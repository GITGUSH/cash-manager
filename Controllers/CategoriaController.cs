public static class CategoriaController
{
    public static void MapearRotas(WebApplication app)
    {
        var service = new CategoriaService();

        app.MapPost("/categoria", (Categoria categoria) =>
        {
            service.Inserir(categoria.Nome!, categoria.TipoES!, categoria.IdUsuario );
            return Results.Ok("Categoria criada com sucesso!");
        });

        app.MapGet("/categorias", () =>
        {
            var lista = service.Listar();
            return Results.Ok(lista);
        });
    }
}