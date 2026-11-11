using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RappyFastFood.API.Dtos;
using RappyFastFood.API.Services;

namespace RappyFastFood.API.Controllers;

[ApiController]
[Route("api/pedidos")]
[Authorize]
public class PedidosController : ControllerBase
{
    private readonly PedidoService _pedidos;

    public PedidosController(PedidoService pedidos)
    {
        _pedidos = pedidos;
    }

    private int IdUsuario => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    private string Tipo => User.FindFirstValue(ClaimTypes.Role)!;

    [Authorize(Roles = "cliente,empresa")]
    [HttpPost]
    public async Task<IActionResult> Criar(CriarPedidoRequest request)
    {
        return Ok(await _pedidos.CriarAsync(IdUsuario, Tipo, request));
    }

    [Authorize(Roles = "cliente,empresa")]
    [HttpGet("meus")]
    public async Task<IActionResult> Meus()
    {
        return Ok(await _pedidos.ListarDoUsuarioAsync(IdUsuario, Tipo));
    }

    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<IActionResult> Todos()
    {
        return Ok(await _pedidos.ListarTodosAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Buscar(int id)
    {
        var pedido = await _pedidos.BuscarAsync(id);
        return pedido == null ? NotFound() : Ok(pedido);
    }

    [Authorize(Roles = "admin")]
    [HttpPut("{id}/status")]
    public async Task<IActionResult> AtualizarStatus(int id, AtualizarStatusRequest request)
    {
        var pedido = await _pedidos.AtualizarStatusAsync(id, request.Status);
        return pedido == null ? NotFound() : Ok(pedido);
    }
}
