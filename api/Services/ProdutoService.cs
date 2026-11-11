using RappyFastFood.API.Dtos;
using RappyFastFood.API.Models;
using RappyFastFood.API.Repositories;

namespace RappyFastFood.API.Services;

public class ProdutoService
{
    private readonly ProdutoRepository _produtos;

    public ProdutoService(ProdutoRepository produtos)
    {
        _produtos = produtos;
    }

    public Task<List<ProdutoLista>> ListarAsync(int? idCategoria) => _produtos.ListarAsync(idCategoria);

    public Task<List<Categoria>> ListarCategoriasAsync() => _produtos.ListarCategoriasAsync();

    public Task<Produto?> BuscarAsync(int id) => _produtos.BuscarAsync(id);

    public async Task<Produto> CriarAsync(ProdutoRequest request)
    {
        Validar(request);
        var produto = new Produto();
        Preencher(produto, request);
        await _produtos.AdicionarAsync(produto);
        return produto;
    }

    public async Task<Produto?> AtualizarAsync(int id, ProdutoRequest request)
    {
        Validar(request);
        var produto = await _produtos.BuscarAsync(id);
        if (produto == null)
        {
            return null;
        }
        Preencher(produto, request);
        await _produtos.SalvarAsync();
        return produto;
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var produto = await _produtos.BuscarAsync(id);
        if (produto == null)
        {
            return false;
        }
        await _produtos.RemoverAsync(produto);
        return true;
    }

    private static void Preencher(Produto produto, ProdutoRequest request)
    {
        produto.Nome = request.Nome;
        produto.Descricao = request.Descricao;
        produto.Preco = request.Preco;
        produto.IdCategoria = request.IdCategoria;
        produto.ImagemUrl = request.ImagemUrl;
        produto.Calorias = request.Calorias;
        produto.Tags = request.Tags;
        produto.Disponivel = request.Disponivel;
    }

    private static void Validar(ProdutoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nome))
        {
            throw new InvalidOperationException("O nome do produto e obrigatorio.");
        }
        if (request.Preco <= 0)
        {
            throw new InvalidOperationException("O preco deve ser maior que zero.");
        }
    }
}
