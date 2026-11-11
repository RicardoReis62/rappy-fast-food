namespace RappyFastFood.API.Models;

public class Usuario : EntidadeBase
{
    private string _email = "";

    public string Email
    {
        get => _email;
        set => _email = value.Trim().ToLowerInvariant();
    }

    public string SenhaHash { get; private set; } = "";
    public string TipoUsuario { get; set; } = "cliente";
    public string Nome { get; set; } = "";
    public string? Telefone { get; set; }
    public bool Ativo { get; set; } = true;
    public DateTime DataCadastro { get; set; } = DateTime.Now;

    public void DefinirSenha(string senha)
    {
        SenhaHash = BCrypt.Net.BCrypt.HashPassword(senha);
    }

    public bool SenhaConfere(string senha)
    {
        return BCrypt.Net.BCrypt.Verify(senha, SenhaHash);
    }
}
