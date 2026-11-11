using Microsoft.EntityFrameworkCore;
using RappyFastFood.API.Data;
using RappyFastFood.API.Dtos;
using RappyFastFood.API.Models;

namespace RappyFastFood.API.Repositories;

public class ProdutoRepository
{
    private readonly AppDbContext _db;

    public ProdutoRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<List<ProdutoLista>> ListarAsync(int? idCategoria)
    {
        return _db.Database
            .SqlQuery<ProdutoLista>($"EXEC sp_ProdutosPorCategoria {idCategoria}")
            .ToListAsync();
    }

    public Task<List<Categoria>> ListarCategoriasAsync()
    {
        return _db.Categorias.Where(c => c.Ativo).OrderBy(c => c.Nome).ToListAsync();
    }

    public Task<Produto?> BuscarAsync(int id)
    {
        return _db.Produtos.Include(p => p.Categoria).FirstOrDefaultAsync(p => p.Id == id);
    }

    public Task<List<Produto>> BuscarPorIdsAsync(List<int> ids)
    {
        return _db.Produtos.Where(p => ids.Contains(p.Id) && p.Disponivel).ToListAsync();
    }

    public async Task AdicionarAsync(Produto produto)
    {
        _db.Produtos.Add(produto);
        await _db.SaveChangesAsync();
    }

    public async Task SalvarAsync()
    {
        await _db.SaveChangesAsync();
    }

    public async Task RemoverAsync(Produto produto)
    {
        var usado = await _db.ItensPedido.AnyAsync(i => i.IdProduto == produto.Id);
        if (usado)
        {
            produto.Disponivel = false;
        }
        else
        {
            _db.Produtos.Remove(produto);
        }
        await _db.SaveChangesAsync();
    }
}
