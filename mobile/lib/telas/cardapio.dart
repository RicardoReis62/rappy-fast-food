import 'package:flutter/material.dart';

import '../api.dart';
import '../modelos.dart';
import '../sessao.dart';

class TelaCardapio extends StatefulWidget {
  const TelaCardapio({super.key});

  @override
  State<TelaCardapio> createState() => _TelaCardapioState();
}

class _TelaCardapioState extends State<TelaCardapio> {
  List<Categoria> categorias = [];
  List<Produto> produtos = [];
  int? categoriaAtual;
  bool carregando = true;
  String? erro;

  @override
  void initState() {
    super.initState();
    carregar();
  }

  Future<void> carregar() async {
    setState(() {
      carregando = true;
      erro = null;
    });
    try {
      categorias = await Api.categorias();
      produtos = await Api.produtos(categoriaAtual);
    } catch (e) {
      erro = 'Nao foi possivel carregar o cardapio.';
    }
    setState(() => carregando = false);
  }

  Future<void> filtrar(int? id) async {
    categoriaAtual = id;
    await carregar();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Cardapio')),
      body: Column(
        children: [
          SizedBox(
            height: 56,
            child: ListView(
              scrollDirection: Axis.horizontal,
              padding: const EdgeInsets.symmetric(horizontal: 8),
              children: [
                Padding(
                  padding: const EdgeInsets.all(4),
                  child: ChoiceChip(
                    label: const Text('Todos'),
                    selected: categoriaAtual == null,
                    onSelected: (_) => filtrar(null),
                  ),
                ),
                for (final c in categorias)
                  Padding(
                    padding: const EdgeInsets.all(4),
                    child: ChoiceChip(
                      label: Text(c.nome),
                      selected: categoriaAtual == c.id,
                      onSelected: (_) => filtrar(c.id),
                    ),
                  ),
              ],
            ),
          ),
          Expanded(
            child: carregando
                ? const Center(child: CircularProgressIndicator())
                : erro != null
                    ? Center(child: Text(erro!))
                    : RefreshIndicator(
                        onRefresh: carregar,
                        child: ListView.builder(
                          itemCount: produtos.length,
                          itemBuilder: (context, indice) {
                            final produto = produtos[indice];
                            return Card(
                              margin: const EdgeInsets.symmetric(horizontal: 12, vertical: 6),
                              child: ListTile(
                                title: Text(produto.nome),
                                subtitle: Text('${produto.descricao}\n${moeda(produto.preco)}'),
                                isThreeLine: true,
                                trailing: IconButton(
                                  tooltip: 'Adicionar ao carrinho',
                                  icon: const Icon(Icons.add_shopping_cart),
                                  onPressed: () {
                                    Sessao.instancia.adicionar(produto);
                                    ScaffoldMessenger.of(context).showSnackBar(
                                      SnackBar(content: Text('${produto.nome} adicionado')),
                                    );
                                  },
                                ),
                              ),
                            );
                          },
                        ),
                      ),
          ),
        ],
      ),
    );
  }
}
