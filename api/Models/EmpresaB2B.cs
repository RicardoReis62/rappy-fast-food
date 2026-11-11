namespace RappyFastFood.API.Models;

public class EmpresaB2B : EntidadeBase
{
    public int IdUsuario { get; set; }
    public Usuario Usuario { get; set; } = null!;
    public string? RazaoSocial { get; set; }
    public string? CNPJ { get; set; }
    public string? NomeFantasia { get; set; }
    public int? QuantidadeFuncionarios { get; set; }
    public string? EnderecoEntrega { get; set; }
}
