import 'package:flutter/foundation.dart';

import 'modelos.dart';

class Sessao extends ChangeNotifier {
  Sessao._();

  static final Sessao instancia = Sessao._();

  Usuario? usuario;
  final List<ItemCarrinho> carrinho = [];

  bool get logado => usuario != null;
  int get totalItens => carrinho.fold(0, (soma, i) => soma + i.quantidade);
  double get subtotal => carrinho.fold(0, (soma, i) => soma + i.quantidade * i.produto.preco);

  void entrar(Usuario novo) {
    usuario = novo;
    notifyListeners();
  }

  void sair() {
    usuario = null;
    carrinho.clear();
    notifyListeners();
  }

  void adicionar(Produto produto) {
    final existente = carrinho.where((i) => i.produto.id == produto.id);
    if (existente.isEmpty) {
      carrinho.add(ItemCarrinho(produto: produto));
    } else {
      existente.first.quantidade++;
    }
    notifyListeners();
  }

  void alterar(ItemCarrinho item, int delta) {
    item.quantidade += delta;
    if (item.quantidade <= 0) {
      carrinho.remove(item);
    }
    notifyListeners();
  }

  void limparCarrinho() {
    carrinho.clear();
    notifyListeners();
  }
}
