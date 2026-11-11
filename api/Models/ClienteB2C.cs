namespace RappyFastFood.API.Models;

public class ClienteB2C : EntidadeBase
{
    public int IdUsuario { get; set; }
    public Usuario Usuario { get; set; } = null!;
    public string? CPF { get; set; }
    public string? Endereco { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
}
