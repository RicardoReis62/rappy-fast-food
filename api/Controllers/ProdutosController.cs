using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RappyFastFood.API.Dtos;
using RappyFastFood.API.Services;

namespace RappyFastFood.API.Controllers;

[ApiController]
[Route("api")]
public class ProdutosController : ControllerBase
{
    private readonly ProdutoService _produtos;

    public ProdutosController(ProdutoService produtos)
    {
        _produtos = produtos;
    }

    [HttpGet("categorias")]
    public async Task<IActionResult> Categorias()
    {
        return Ok(await _produtos.ListarCategoriasAsync());
    }

    [HttpGet("produtos")]
    public async Task<IActionResult> Listar([FromQuery] int? categoriaId)
    {
        return Ok(await _produtos.ListarAsync(categoriaId));
    }

    [HttpGet("produtos/{id}")]
    public async Task<IActionResult> Buscar(int id)
    {
        var produto = await _produtos.BuscarAsync(id);
        return produto == null ? NotFound() : Ok(produto);
    }

    [Authorize(Roles = "admin")]
    [HttpPost("produtos")]
    public async Task<IActionResult> Criar(ProdutoRequest request)
    {
        var produto = await _produtos.CriarAsync(request);
        return CreatedAtAction(nameof(Buscar), new { id = produto.Id }, produto);
    }

    [Authorize(Roles = "admin")]
    [HttpPut("produtos/{id}")]
    public async Task<IActionResult> Atualizar(int id, ProdutoRequest request)
    {
        var produto = await _produtos.AtualizarAsync(id, request);
        return produto == null ? NotFound() : Ok(produto);
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("produtos/{id}")]
    public async Task<IActionResult> Remover(int id)
    {
        return await _produtos.RemoverAsync(id) ? NoContent() : NotFound();
    }
}
