public class Usuario
{
    public int IdUsuario {get; set;}
    public string? Nome {get; set; }
    public string? Email { get; set; }
    public string? SenhaHash { get; set; }
    public DateTime DataInclusao { get; set; }
}