using RappyFastFood.Web.Models;

namespace RappyFastFood.Web.Services;

public class Sessao
{
    public AuthResposta? Usuario { get; private set; }
    public List<ItemCarrinho> Carrinho { get; } = new();

    public event Action? Alterou;

    public string? Token => Usuario?.Token;
    public bool Logado => Usuario != null;
    public bool EhAdmin => Usuario?.TipoUsuario == "admin";
    public int TotalItens => Carrinho.Sum(i => i.Quantidade);
    public decimal Subtotal => Carrinho.Sum(i => i.Quantidade * i.Produto.Preco);

    public void Entrar(AuthResposta usuario)
    {
        Usuario = usuario;
        Alterou?.Invoke();
    }

    public void Sair()
    {
        Usuario = null;
        Carrinho.Clear();
        Alterou?.Invoke();
    }

    public void Adicionar(Produto produto)
    {
        var item = Carrinho.FirstOrDefault(i => i.Produto.Id == produto.Id);
        if (item == null)
        {
            Carrinho.Add(new ItemCarrinho { Produto = produto, Quantidade = 1 });
        }
        else
        {
            item.Quantidade++;
        }
        Alterou?.Invoke();
    }

    public void Alterar(int idProduto, int delta)
    {
        var item = Carrinho.FirstOrDefault(i => i.Produto.Id == idProduto);
        if (item == null)
        {
            return;
        }
        item.Quantidade += delta;
        if (item.Quantidade <= 0)
        {
            Carrinho.Remove(item);
        }
        Alterou?.Invoke();
    }

    public void LimparCarrinho()
    {
        Carrinho.Clear();
        Alterou?.Invoke();
    }
}
