namespace RappyFastFood.Web.Models;

public class AuthResposta
{
    public string Token { get; set; } = "";
    public int IdUsuario { get; set; }
    public string Nome { get; set; } = "";
    public string Email { get; set; } = "";
    public string TipoUsuario { get; set; } = "";
}

public class Categoria
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public string? Descricao { get; set; }
}

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public string? ImagemUrl { get; set; }
    public int? Calorias { get; set; }
    public string? Tags { get; set; }
    public string? Categoria { get; set; }
    public int? IdCategoria { get; set; }
    public bool Disponivel { get; set; } = true;
}

public class ItemPedidoResposta
{
    public string Produto { get; set; } = "";
    public int Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
}

public class Pedido
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

public class Resumo
{
    public int PedidosHoje { get; set; }
    public decimal FaturamentoHoje { get; set; }
    public int ClientesAtivos { get; set; }
    public decimal TicketMedio { get; set; }
    public int IngredientesEmAlerta { get; set; }
}

public class VendaDia
{
    public DateTime Dia { get; set; }
    public int Pedidos { get; set; }
    public decimal Faturamento { get; set; }
    public decimal TicketMedio { get; set; }
}

public class Alerta
{
    public string Mensagem { get; set; } = "";
    public DateTime DataHora { get; set; }
}

public class ItemCarrinho
{
    public Produto Produto { get; set; } = new();
    public int Quantidade { get; set; }
}

public class ErroApi
{
    public string? Erro { get; set; }
}
