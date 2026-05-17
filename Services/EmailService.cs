using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
public class EmailService
{
    public void EnviarPdf(string destinatario, string nomeDestinatario, string assunto, string pdfBase64)
    {
        var remetente = Environment.GetEnvironmentVariable("EMAIL_USUARIO")!;
        var senha     = Environment.GetEnvironmentVariable("EMAIL_SENHA")!;

        var mensagem = new MimeMessage();
        mensagem.From.Add(new MailboxAddress("CashManager", remetente));
        mensagem.To.Add(new MailboxAddress(nomeDestinatario, destinatario));
        mensagem.Subject = assunto;

        var builder = new BodyBuilder();
        builder.TextBody = $"Olá {nomeDestinatario}, segue em anexo o extrato solicitado.";

        // Anexa o PDF
        var pdfBytes = Convert.FromBase64String(pdfBase64);
        builder.Attachments.Add("extrato.pdf", pdfBytes, ContentType.Parse("application/pdf"));

        mensagem.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        smtp.Authenticate(remetente, senha);
        smtp.Send(mensagem);
        smtp.Disconnect(true);
    }
}