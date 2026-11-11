using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RappyFastFood.API.Services;

namespace RappyFastFood.API.Controllers;

[ApiController]
[Route("api/dashboard")]
[Authorize(Roles = "admin")]
public class DashboardController : ControllerBase
{
    private readonly PedidoService _pedidos;

    public DashboardController(PedidoService pedidos)
    {
        _pedidos = pedidos;
    }

    [HttpGet("resumo")]
    public async Task<IActionResult> Resumo()
    {
        return Ok(await _pedidos.ResumoAsync());
    }

    [HttpGet("vendas")]
    public async Task<IActionResult> Vendas([FromQuery] DateTime? inicio, [FromQuery] DateTime? fim)
    {
        var fimPadrao = fim ?? DateTime.Today;
        var inicioPadrao = inicio ?? fimPadrao.AddDays(-6);
        return Ok(await _pedidos.VendasAsync(inicioPadrao, fimPadrao));
    }

    [HttpGet("alertas")]
    public async Task<IActionResult> Alertas()
    {
        return Ok(await _pedidos.AlertasAsync());
    }
}
