using System.Net.Http.Headers;
using System.Net.Http.Json;
using RappyFastFood.Web.Models;

namespace RappyFastFood.Web.Services;

public class ApiClient
{
    private readonly HttpClient _http;
    private readonly Sessao _sessao;

    public ApiClient(HttpClient http, Sessao sessao)
    {
        _http = http;
        _sessao = sessao;
    }

    private async Task<HttpResponseMessage> Enviar(HttpMethod metodo, string url, object? corpo = null)
    {
        var requisicao = new HttpRequestMessage(metodo, url);
        if (_sessao.Token != null)
        {
            requisicao.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _sessao.Token);
        }
        if (corpo != null)
        {
            requisicao.Content = JsonContent.Create(corpo);
        }

        var resposta = await _http.SendAsync(requisicao);
        if (!resposta.IsSuccessStatusCode)
        {
            var mensagem = "Nao foi possivel concluir a operacao.";
            try
            {
                var erro = await resposta.Content.ReadFromJsonAsync<ErroApi>();
                if (!string.IsNullOrEmpty(erro?.Erro))
                {
                    mensagem = erro.Erro;
                }
            }
            catch
            {
            }
            throw new Exception(mensagem);
        }
        return resposta;
    }

    private async Task<T> Obter<T>(HttpMethod metodo, string url, object? corpo = null)
    {
        var resposta = await Enviar(metodo, url, corpo);
        return (await resposta.Content.ReadFromJsonAsync<T>())!;
    }

    public Task<AuthResposta> Login(string email, string senha) =>
        Obter<AuthResposta>(HttpMethod.Post, "api/auth/login", new { email, senha });

    public Task<AuthResposta> CadastrarCliente(object dados) =>
        Obter<AuthResposta>(HttpMethod.Post, "api/auth/cadastro/cliente", dados);

    public Task<AuthResposta> CadastrarEmpresa(object dados) =>
        Obter<AuthResposta>(HttpMethod.Post, "api/auth/cadastro/empresa", dados);

    public Task<List<Categoria>> Categorias() =>
        Obter<List<Categoria>>(HttpMethod.Get, "api/categorias");

    public Task<List<Produto>> Produtos(int? categoriaId = null) =>
        Obter<List<Produto>>(HttpMethod.Get, categoriaId == null ? "api/produtos" : $"api/produtos?categoriaId={categoriaId}");

    public Task<Produto> Produto(int id) =>
        Obter<Produto>(HttpMethod.Get, $"api/produtos/{id}");

    public Task<Produto> SalvarProduto(int? id, object dados) =>
        id == null
            ? Obter<Produto>(HttpMethod.Post, "api/produtos", dados)
            : Obter<Produto>(HttpMethod.Put, $"api/produtos/{id}", dados);

    public Task RemoverProduto(int id) => Enviar(HttpMethod.Delete, $"api/produtos/{id}");

    public Task<Pedido> CriarPedido(object dados) =>
        Obter<Pedido>(HttpMethod.Post, "api/pedidos", dados);

    public Task<List<Pedido>> MeusPedidos() =>
        Obter<List<Pedido>>(HttpMethod.Get, "api/pedidos/meus");

    public Task<List<Pedido>> TodosPedidos() =>
        Obter<List<Pedido>>(HttpMethod.Get, "api/pedidos");

    public Task<Pedido> AtualizarStatus(int id, string status) =>
        Obter<Pedido>(HttpMethod.Put, $"api/pedidos/{id}/status", new { status });

    public Task<Resumo> Resumo() =>
        Obter<Resumo>(HttpMethod.Get, "api/dashboard/resumo");

    public Task<List<VendaDia>> Vendas() =>
        Obter<List<VendaDia>>(HttpMethod.Get, "api/dashboard/vendas");

    public Task<List<Alerta>> Alertas() =>
        Obter<List<Alerta>>(HttpMethod.Get, "api/dashboard/alertas");
}
