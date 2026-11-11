using RappyFastFood.API.Dtos;
using RappyFastFood.API.Models;
using RappyFastFood.API.Repositories;

namespace RappyFastFood.API.Services;

public class AuthService
{
    private readonly UsuarioRepository _usuarios;
    private readonly TokenService _tokens;

    public AuthService(UsuarioRepository usuarios, TokenService tokens)
    {
        _usuarios = usuarios;
        _tokens = tokens;
    }

    private AuthResposta Montar(Usuario usuario)
    {
        return new AuthResposta
        {
            Token = _tokens.Gerar(usuario),
            IdUsuario = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            TipoUsuario = usuario.TipoUsuario
        };
    }

    public async Task<AuthResposta?> LoginAsync(LoginRequest request)
    {
        var usuario = await _usuarios.BuscarPorEmailAsync(request.Email);
        if (usuario == null || !usuario.Ativo || !usuario.SenhaConfere(request.Senha))
        {
            return null;
        }
        return Montar(usuario);
    }

    public async Task<AuthResposta> CadastrarClienteAsync(CadastroClienteRequest request)
    {
        Validar(request.Nome, request.Email, request.Senha);
        if (await _usuarios.EmailExisteAsync(request.Email))
        {
            throw new InvalidOperationException("E-mail ja cadastrado.");
        }

        var usuario = new Usuario
        {
            Email = request.Email,
            Nome = request.Nome,
            Telefone = request.Telefone,
            TipoUsuario = "cliente"
        };
        usuario.DefinirSenha(request.Senha);

        await _usuarios.AdicionarClienteAsync(new ClienteB2C
        {
            Usuario = usuario,
            CPF = request.CPF,
            Endereco = request.Endereco,
            Cidade = request.Cidade,
            Estado = request.Estado
        });

        return Montar(usuario);
    }

    public async Task<AuthResposta> CadastrarEmpresaAsync(CadastroEmpresaRequest request)
    {
        Validar(request.Nome, request.Email, request.Senha);
        if (await _usuarios.EmailExisteAsync(request.Email))
        {
            throw new InvalidOperationException("E-mail ja cadastrado.");
        }
        if (!string.IsNullOrWhiteSpace(request.CNPJ) && await _usuarios.CnpjExisteAsync(request.CNPJ))
        {
            throw new InvalidOperationException("CNPJ ja cadastrado.");
        }

        var usuario = new Usuario
        {
            Email = request.Email,
            Nome = request.Nome,
            Telefone = request.Telefone,
            TipoUsuario = "empresa"
        };
        usuario.DefinirSenha(request.Senha);

        await _usuarios.AdicionarEmpresaAsync(new EmpresaB2B
        {
            Usuario = usuario,
            RazaoSocial = request.RazaoSocial,
            NomeFantasia = request.NomeFantasia,
            CNPJ = string.IsNullOrWhiteSpace(request.CNPJ) ? null : request.CNPJ,
            QuantidadeFuncionarios = request.QuantidadeFuncionarios,
            EnderecoEntrega = request.EnderecoEntrega
        });

        return Montar(usuario);
    }

    private static void Validar(string nome, string email, string senha)
    {
        if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
        {
            throw new InvalidOperationException("Nome e e-mail validos sao obrigatorios.");
        }
        if (senha.Length < 6)
        {
            throw new InvalidOperationException("A senha deve ter ao menos 6 caracteres.");
        }
    }
}
