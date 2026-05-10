public static class UsuarioController
{
    public static void MapearRotas(WebApplication app)
    {
        var service = new UsuarioService();

        app.MapPost("/usuario", (Usuario usuario) =>
        {
            var service = new UsuarioService();
            var cadastrou = service.Inserir(usuario.Nome!, usuario.Email!, usuario.SenhaHash!);

            if (!cadastrou)
                return Results.BadRequest("Este e-mail já está cadastrado.");

            return Results.Ok("Usuário criado com sucesso!");
        });

        app.MapGet("/usuarios", () =>
        {
            var lista = service.Listar();
            return Results.Ok(lista);
        });

    }
}