namespace RappyFastFood.API.Models;

public class Ingrediente : EntidadeBase
{
    public string Nome { get; set; } = "";
    public string? UnidadeMedida { get; set; }
    public decimal QuantidadeAtual { get; set; }
    public decimal QuantidadeMinima { get; set; }
    public decimal? PrecoUnitario { get; set; }
    public int? IdRestaurante { get; set; }
}
