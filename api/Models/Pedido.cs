namespace RappyFastFood.API.Models;

public class Pedido : EntidadeBase
{
    public string TipoCliente { get; set; } = "B2C";
    public int? IdClienteB2C { get; set; }
    public ClienteB2C? ClienteB2C { get; set; }
    public int? IdEmpresaB2B { get; set; }
    public EmpresaB2B? EmpresaB2B { get; set; }
    public DateTime DataHora { get; set; } = DateTime.Now;
    public string TipoEntrega { get; set; } = "Delivery";
    public string Status { get; set; } = "Recebido";
    public string? EnderecoEntrega { get; set; }
    public decimal ValorTotal { get; set; }
    public string? FormaPagamento { get; set; }
    public string? Observacoes { get; set; }
    public decimal TaxaEntrega { get; set; }
    public decimal Desconto { get; set; }
    public List<ItemPedido> Itens { get; set; } = new();
}
