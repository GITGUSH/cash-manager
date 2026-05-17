public static class EmailController
{
    public static void MapearRotas(WebApplication app)
    {
        var service = new EmailService();

        app.MapPost("/email/extrato", (EmailRequest request) =>
        {
            service.EnviarPdf(request.Destinatario!, request.NomeDestinatario!, request.AssuntoPdf!, request.PdfBase64!);
            return Results.Ok("Email enviado com sucesso!");
        }).RequireAuthorization();
    }
}