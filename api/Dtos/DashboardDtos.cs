namespace RappyFastFood.API.Dtos;

public class ResumoDashboard
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
