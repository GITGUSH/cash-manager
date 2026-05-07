using Superpower.Parsers;

public class Operacao
{
    public int IdOperacao {get; set; }
    public string? Descricao {get; set; }
    public decimal Valor {get; set; }
    public string? TipoES {get; set; }
    public DateTime DataOperacao {get; set; }
    public int IdConta {get; set; }
    public int IdCategoria {get; set; }
}