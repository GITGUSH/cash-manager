public static class LoginController
{
    public static void MapearRotas(WebApplication app)
    {
        var service = new LoginService();

        app.MapPost("/login", (LoginRequest request) =>
        {
            var token = service.Login(request.Email!, request.Senha!);

            if (token == null)
            {
                return Results.Unauthorized();
            }
            return Results.Ok(new { token });
        });
    }
}