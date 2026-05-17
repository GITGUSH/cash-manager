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

        app.MapGet("/usuario/perfil", (HttpContext http) =>
        {
            var service   = new UsuarioService();
            var idUsuario = int.Parse(http.User.FindFirst("id")!.Value);
            var usuario   = service.BuscarPorId(idUsuario);
        
            if (usuario == null) return Results.NotFound();
            return Results.Ok(usuario);
        }).RequireAuthorization();

        app.MapGet("/usuarios", () =>
        {
            var lista = service.Listar();
            return Results.Ok(lista);
        });

    }
}