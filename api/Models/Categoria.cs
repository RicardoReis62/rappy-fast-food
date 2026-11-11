namespace RappyFastFood.API.Models;

public class Categoria : EntidadeBase
{
    public string Nome { get; set; } = "";
    public string? Descricao { get; set; }
    public string TipoPublico { get; set; } = "Ambos";
    public bool Ativo { get; set; } = true;
}
