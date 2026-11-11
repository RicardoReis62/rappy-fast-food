namespace RappyFastFood.API.Dtos;

public class ItemPedidoRequest
{
    public int IdProduto { get; set; }
    public int Quantidade { get; set; }
}

public class CriarPedidoRequest
{
    public string? EnderecoEntrega { get; set; }
    public string FormaPagamento { get; set; } = "PIX";
    public string? Observacoes { get; set; }
    public List<ItemPedidoRequest> Itens { get; set; } = new();
}

public class AtualizarStatusRequest
{
    public string Status { get; set; } = "";
}

public class ItemPedidoResposta
{
    public string Produto { get; set; } = "";
    public int Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
}

public class PedidoResposta
{
    public int Id { get; set; }
    public DateTime DataHora { get; set; }
    public string Status { get; set; } = "";
    public string TipoCliente { get; set; } = "";
    public string? Cliente { get; set; }
    public string? EnderecoEntrega { get; set; }
    public string? FormaPagamento { get; set; }
    public decimal TaxaEntrega { get; set; }
    public decimal Desconto { get; set; }
    public decimal ValorTotal { get; set; }
    public List<ItemPedidoResposta> Itens { get; set; } = new();
}
