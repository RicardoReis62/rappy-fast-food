class Categoria {
  final int id;
  final String nome;

  Categoria({required this.id, required this.nome});

  factory Categoria.fromJson(Map<String, dynamic> json) {
    return Categoria(id: json['id'], nome: json['nome']);
  }
}

class Produto {
  final int id;
  final String nome;
  final String descricao;
  final double preco;
  final String categoria;
  final int? calorias;

  Produto({
    required this.id,
    required this.nome,
    required this.descricao,
    required this.preco,
    required this.categoria,
    this.calorias,
  });

  factory Produto.fromJson(Map<String, dynamic> json) {
    return Produto(
      id: json['id'],
      nome: json['nome'],
      descricao: json['descricao'] ?? '',
      preco: (json['preco'] as num).toDouble(),
      categoria: json['categoria'] ?? '',
      calorias: json['calorias'],
    );
  }
}

class ItemCarrinho {
  final Produto produto;
  int quantidade;

  ItemCarrinho({required this.produto, this.quantidade = 1});
}

class ItemPedido {
  final String produto;
  final int quantidade;

  ItemPedido({required this.produto, required this.quantidade});
}

class Pedido {
  final int id;
  final DateTime dataHora;
  final String status;
  final double valorTotal;
  final List<ItemPedido> itens;

  Pedido({
    required this.id,
    required this.dataHora,
    required this.status,
    required this.valorTotal,
    required this.itens,
  });

  factory Pedido.fromJson(Map<String, dynamic> json) {
    return Pedido(
      id: json['id'],
      dataHora: DateTime.parse(json['dataHora']),
      status: json['status'],
      valorTotal: (json['valorTotal'] as num).toDouble(),
      itens: (json['itens'] as List)
          .map((i) => ItemPedido(produto: i['produto'], quantidade: i['quantidade']))
          .toList(),
    );
  }
}

class Usuario {
  final String token;
  final String nome;
  final String email;
  final String tipo;

  Usuario({required this.token, required this.nome, required this.email, required this.tipo});

  factory Usuario.fromJson(Map<String, dynamic> json) {
    return Usuario(
      token: json['token'],
      nome: json['nome'],
      email: json['email'],
      tipo: json['tipoUsuario'],
    );
  }
}

String moeda(double valor) {
  return 'R\$ ${valor.toStringAsFixed(2).replaceAll('.', ',')}';
}
