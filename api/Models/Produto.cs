namespace RappyFastFood.API.Models;

public class Produto : EntidadeBase
{
    public string Nome { get; set; } = "";
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public decimal? CustoProducao { get; set; }
    public int? IdCategoria { get; set; }
    public Categoria? Categoria { get; set; }
    public string? ImagemUrl { get; set; }
    public int? TempoPreparoMinutos { get; set; }
    public bool Disponivel { get; set; } = true;
    public bool EhKitSemanal { get; set; }
    public bool EhParaEvento { get; set; }
    public int? QuantidadeMinimaEvento { get; set; }
    public int? IdRestaurante { get; set; }
    public int? Calorias { get; set; }
    public string? Tags { get; set; }
    public DateTime DataCadastro { get; set; } = DateTime.Now;
}
