public static class UsuarioController
{
    public static void MapearRotas(WebApplication app)
    {
        var service = new UsuarioService();

        app.MapPost("/usuario", (Usuario usuario) =>
        {
            service.Inserir(usuario.Nome!, usuario.Email!, usuario.SenhaHash!);
            return Results.Ok("Usuário criado com sucesso!");
        });

        app.MapGet("/usuarios", () =>
        {
            var lista = service.Listar();
            return Results.Ok(lista);
        });
    }
}