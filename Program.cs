


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

UsuarioController.MapearRotas(app);

app.Run();

