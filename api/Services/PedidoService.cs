using RappyFastFood.API.Dtos;
using RappyFastFood.API.Models;
using RappyFastFood.API.Repositories;

namespace RappyFastFood.API.Services;

public class PedidoService
{
    private const decimal TaxaEntregaPadrao = 5.00m;
    private const decimal DescontoEmpresa = 0.10m;
    private static readonly string[] FormasPagamento = { "PIX", "CartaoCredito", "CartaoDebito", "Dinheiro" };

    private readonly PedidoRepository _pedidos;
    private readonly ProdutoRepository _produtos;
    private readonly UsuarioRepository _usuarios;

    public PedidoService(PedidoRepository pedidos, ProdutoRepository produtos, UsuarioRepository usuarios)
    {
        _pedidos = pedidos;
        _produtos = produtos;
        _usuarios = usuarios;
    }

    public async Task<PedidoResposta> CriarAsync(int idUsuario, string tipoUsuario, CriarPedidoRequest request)
    {
        if (request.Itens.Count == 0 || request.Itens.Any(i => i.Quantidade <= 0))
        {
            throw new InvalidOperationException("O pedido precisa ter itens com quantidade valida.");
        }
        if (!FormasPagamento.Contains(request.FormaPagamento))
        {
            throw new InvalidOperationException("Forma de pagamento invalida.");
        }

        var pedido = new Pedido
        {
            EnderecoEntrega = request.EnderecoEntrega,
            FormaPagamento = request.FormaPagamento,
            Observacoes = request.Observacoes,
            TaxaEntrega = TaxaEntregaPadrao,
            DataHora = DateTime.Now
        };

        if (tipoUsuario == "empresa")
        {
            var empresa = await _usuarios.BuscarEmpresaPorUsuarioAsync(idUsuario)
                ?? throw new InvalidOperationException("Empresa nao encontrada.");
            pedido.TipoCliente = "B2B";
            pedido.IdEmpresaB2B = empresa.Id;
            pedido.EnderecoEntrega ??= empresa.EnderecoEntrega;
        }
        else
        {
            var cliente = await _usuarios.BuscarClientePorUsuarioAsync(idUsuario)
                ?? throw new InvalidOperationException("Cliente nao encontrado.");
            pedido.TipoCliente = "B2C";
            pedido.IdClienteB2C = cliente.Id;
            pedido.EnderecoEntrega ??= cliente.Endereco;
        }

        var ids = request.Itens.Select(i => i.IdProduto).Distinct().ToList();
        var produtos = await _produtos.BuscarPorIdsAsync(ids);
        if (produtos.Count != ids.Count)
        {
            throw new InvalidOperationException("Existem produtos indisponiveis no pedido.");
        }

        foreach (var item in request.Itens)
        {
            var produto = produtos.First(p => p.Id == item.IdProduto);
            pedido.Itens.Add(new ItemPedido
            {
                IdProduto = produto.Id,
                Quantidade = item.Quantidade,
                ValorUnitario = produto.Preco
            });
        }

        var subtotal = pedido.Itens.Sum(i => i.Quantidade * i.ValorUnitario);
        pedido.Desconto = pedido.TipoCliente == "B2B" ? Math.Round(subtotal * DescontoEmpresa, 2) : 0;
        pedido.ValorTotal = subtotal + pedido.TaxaEntrega - pedido.Desconto;

        await _pedidos.AdicionarAsync(pedido);
        return (await _pedidos.BuscarAsync(pedido.Id))!;
    }

    public async Task<List<PedidoResposta>> ListarDoUsuarioAsync(int idUsuario, string tipoUsuario)
    {
        if (tipoUsuario == "empresa")
        {
            var empresa = await _usuarios.BuscarEmpresaPorUsuarioAsync(idUsuario);
            return empresa == null ? new() : await _pedidos.ListarDoClienteAsync(null, empresa.Id);
        }

        var cliente = await _usuarios.BuscarClientePorUsuarioAsync(idUsuario);
        return cliente == null ? new() : await _pedidos.ListarDoClienteAsync(cliente.Id, null);
    }

    public Task<List<PedidoResposta>> ListarTodosAsync() => _pedidos.ListarTodosAsync();

    public Task<PedidoResposta?> BuscarAsync(int id) => _pedidos.BuscarAsync(id);

    public async Task<PedidoResposta?> AtualizarStatusAsync(int id, string status)
    {
        await _pedidos.AtualizarStatusAsync(id, status);
        return await _pedidos.BuscarAsync(id);
    }

    public Task<ResumoDashboard> ResumoAsync() => _pedidos.ResumoAsync();

    public Task<List<VendaDia>> VendasAsync(DateTime inicio, DateTime fim) => _pedidos.VendasAsync(inicio, fim);

    public Task<List<AlertaEstoque>> AlertasAsync() => _pedidos.AlertasAsync();
}
