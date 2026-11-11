namespace RappyFastFood.API.Dtos;

public class LoginRequest
{
    public string Email { get; set; } = "";
    public string Senha { get; set; } = "";
}

public class CadastroClienteRequest
{
    public string Nome { get; set; } = "";
    public string Email { get; set; } = "";
    public string Senha { get; set; } = "";
    public string? Telefone { get; set; }
    public string? CPF { get; set; }
    public string? Endereco { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
}

public class CadastroEmpresaRequest
{
    public string Nome { get; set; } = "";
    public string Email { get; set; } = "";
    public string Senha { get; set; } = "";
    public string? Telefone { get; set; }
    public string RazaoSocial { get; set; } = "";
    public string NomeFantasia { get; set; } = "";
    public string? CNPJ { get; set; }
    public int QuantidadeFuncionarios { get; set; }
    public string? EnderecoEntrega { get; set; }
}

public class AuthResposta
{
    public string Token { get; set; } = "";
    public int IdUsuario { get; set; }
    public string Nome { get; set; } = "";
    public string Email { get; set; } = "";
    public string TipoUsuario { get; set; } = "";
}
