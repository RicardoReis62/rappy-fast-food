namespace RappyFastFood.API.Models;

public class ItemPedido : EntidadeBase
{
    public int IdPedido { get; set; }
    public int IdProduto { get; set; }
    public Produto Produto { get; set; } = null!;
    public int Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public string? Observacao { get; set; }
}
