import 'dart:convert';

import 'package:http/http.dart' as http;

import 'modelos.dart';
import 'sessao.dart';

class Api {
  static const String _apiUrl = String.fromEnvironment('API_URL');

  static String get baseUrl => _apiUrl.isNotEmpty ? _apiUrl : 'http://${Uri.base.host}:8080';

  static Map<String, String> _cabecalhos() {
    final token = Sessao.instancia.usuario?.token;
    return {
      'Content-Type': 'application/json',
      if (token != null) 'Authorization': 'Bearer $token',
    };
  }

  static dynamic _tratar(http.Response resposta) {
    if (resposta.statusCode >= 200 && resposta.statusCode < 300) {
      return jsonDecode(utf8.decode(resposta.bodyBytes));
    }
    var mensagem = 'Nao foi possivel concluir a operacao.';
    try {
      final corpo = jsonDecode(utf8.decode(resposta.bodyBytes));
      if (corpo['erro'] != null) {
        mensagem = corpo['erro'];
      }
    } catch (_) {}
    throw Exception(mensagem);
  }

  static Future<Usuario> login(String email, String senha) async {
    final resposta = await http.post(
      Uri.parse('$baseUrl/api/auth/login'),
      headers: _cabecalhos(),
      body: jsonEncode({'email': email, 'senha': senha}),
    );
    return Usuario.fromJson(_tratar(resposta));
  }

  static Future<Usuario> cadastrar(Map<String, dynamic> dados) async {
    final resposta = await http.post(
      Uri.parse('$baseUrl/api/auth/cadastro/cliente'),
      headers: _cabecalhos(),
      body: jsonEncode(dados),
    );
    return Usuario.fromJson(_tratar(resposta));
  }

  static Future<List<Categoria>> categorias() async {
    final resposta = await http.get(Uri.parse('$baseUrl/api/categorias'), headers: _cabecalhos());
    return (_tratar(resposta) as List).map((j) => Categoria.fromJson(j)).toList();
  }

  static Future<List<Produto>> produtos(int? categoriaId) async {
    final url = categoriaId == null
        ? '$baseUrl/api/produtos'
        : '$baseUrl/api/produtos?categoriaId=$categoriaId';
    final resposta = await http.get(Uri.parse(url), headers: _cabecalhos());
    return (_tratar(resposta) as List).map((j) => Produto.fromJson(j)).toList();
  }

  static Future<Pedido> criarPedido(String endereco, String pagamento) async {
    final resposta = await http.post(
      Uri.parse('$baseUrl/api/pedidos'),
      headers: _cabecalhos(),
      body: jsonEncode({
        'enderecoEntrega': endereco.isEmpty ? null : endereco,
        'formaPagamento': pagamento,
        'itens': Sessao.instancia.carrinho
            .map((i) => {'idProduto': i.produto.id, 'quantidade': i.quantidade})
            .toList(),
      }),
    );
    return Pedido.fromJson(_tratar(resposta));
  }

  static Future<List<Pedido>> meusPedidos() async {
    final resposta = await http.get(Uri.parse('$baseUrl/api/pedidos/meus'), headers: _cabecalhos());
    return (_tratar(resposta) as List).map((j) => Pedido.fromJson(j)).toList();
  }
}
