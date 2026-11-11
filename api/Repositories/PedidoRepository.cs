using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RappyFastFood.API.Data;
using RappyFastFood.API.Dtos;
using RappyFastFood.API.Models;

namespace RappyFastFood.API.Repositories;

public class PedidoRepository
{
    private readonly AppDbContext _db;

    public PedidoRepository(AppDbContext db)
    {
        _db = db;
    }

    private IQueryable<PedidoResposta> Consulta()
    {
        return _db.Pedidos.Select(p => new PedidoResposta
        {
            Id = p.Id,
            DataHora = p.DataHora,
            Status = p.Status,
            TipoCliente = p.TipoCliente,
            Cliente = p.ClienteB2C != null ? p.ClienteB2C.Usuario.Nome : p.EmpresaB2B!.NomeFantasia,
            EnderecoEntrega = p.EnderecoEntrega,
            FormaPagamento = p.FormaPagamento,
            TaxaEntrega = p.TaxaEntrega,
            Desconto = p.Desconto,
            ValorTotal = p.ValorTotal,
            Itens = p.Itens.Select(i => new ItemPedidoResposta
            {
                Produto = i.Produto.Nome,
                Quantidade = i.Quantidade,
                ValorUnitario = i.ValorUnitario
            }).ToList()
        });
    }

    public async Task AdicionarAsync(Pedido pedido)
    {
        _db.Pedidos.Add(pedido);
        await _db.SaveChangesAsync();
    }

    public Task<PedidoResposta?> BuscarAsync(int id)
    {
        return Consulta().FirstOrDefaultAsync(p => p.Id == id);
    }

    public Task<List<PedidoResposta>> ListarTodosAsync()
    {
        return Consulta().OrderByDescending(p => p.DataHora).Take(100).ToListAsync();
    }

    public Task<List<PedidoResposta>> ListarDoClienteAsync(int? idCliente, int? idEmpresa)
    {
        var consulta = _db.Pedidos
            .Where(p => (idCliente != null && p.IdClienteB2C == idCliente) || (idEmpresa != null && p.IdEmpresaB2B == idEmpresa))
            .Select(p => p.Id);

        return Consulta()
            .Where(p => consulta.Contains(p.Id))
            .OrderByDescending(p => p.DataHora)
            .ToListAsync();
    }

    public async Task AtualizarStatusAsync(int id, string status)
    {
        try
        {
            await _db.Database.ExecuteSqlInterpolatedAsync($"EXEC sp_AtualizarStatusPedido {id}, {status}");
        }
        catch (SqlException ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }

    public async Task<ResumoDashboard> ResumoAsync()
    {
        var linhas = await _db.Database.SqlQuery<ResumoDashboard>($"EXEC sp_ResumoDashboard").ToListAsync();
        return linhas.First();
    }

    public Task<List<VendaDia>> VendasAsync(DateTime inicio, DateTime fim)
    {
        return _db.Database.SqlQuery<VendaDia>($"EXEC sp_RelatorioVendas {inicio.Date}, {fim.Date}").ToListAsync();
    }

    public Task<List<AlertaEstoque>> AlertasAsync()
    {
        return _db.AlertasEstoque.OrderByDescending(a => a.DataHora).Take(50).ToListAsync();
    }
}
