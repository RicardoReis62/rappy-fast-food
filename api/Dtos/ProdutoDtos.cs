namespace RappyFastFood.API.Dtos;

public class ProdutoLista
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public string? ImagemUrl { get; set; }
    public int? Calorias { get; set; }
    public string? Tags { get; set; }
    public string? Categoria { get; set; }
}

public class ProdutoRequest
{
    public string Nome { get; set; } = "";
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public int? IdCategoria { get; set; }
    public string? ImagemUrl { get; set; }
    public int? Calorias { get; set; }
    public string? Tags { get; set; }
    public bool Disponivel { get; set; } = true;
}
