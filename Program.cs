
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

UsuarioController.MapearRotas(app);
ContaController.MapearRotas(app);
CategoriaController.MapearRotas(app);
OperacaoController.MapearRotas(app);

app.Run();
