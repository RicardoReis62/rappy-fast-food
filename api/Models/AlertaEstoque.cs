namespace RappyFastFood.API.Models;

public class AlertaEstoque : EntidadeBase
{
    public int IdIngrediente { get; set; }
    public string Mensagem { get; set; } = "";
    public DateTime DataHora { get; set; }
}
