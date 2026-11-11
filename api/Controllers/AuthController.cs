using Microsoft.AspNetCore.Mvc;
using RappyFastFood.API.Dtos;
using RappyFastFood.API.Services;

namespace RappyFastFood.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _auth;

    public AuthController(AuthService auth)
    {
        _auth = auth;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var resposta = await _auth.LoginAsync(request);
        if (resposta == null)
        {
            return Unauthorized(new { erro = "E-mail ou senha invalidos." });
        }
        return Ok(resposta);
    }

    [HttpPost("cadastro/cliente")]
    public async Task<IActionResult> CadastrarCliente(CadastroClienteRequest request)
    {
        return Ok(await _auth.CadastrarClienteAsync(request));
    }

    [HttpPost("cadastro/empresa")]
    public async Task<IActionResult> CadastrarEmpresa(CadastroEmpresaRequest request)
    {
        return Ok(await _auth.CadastrarEmpresaAsync(request));
    }
}
