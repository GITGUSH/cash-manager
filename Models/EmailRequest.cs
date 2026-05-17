// Models/EmailRequest.cs
public class EmailRequest
{
    public string? Destinatario { get; set; }
    public string? NomeDestinatario { get; set; }
    public string? AssuntoPdf { get; set; }
    public string? PdfBase64 { get; set; }
}