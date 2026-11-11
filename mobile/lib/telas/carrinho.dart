import 'package:flutter/material.dart';

import '../api.dart';
import '../modelos.dart';
import '../sessao.dart';

class TelaCarrinho extends StatefulWidget {
  final VoidCallback aoFinalizar;
  final VoidCallback aoPrecisarLogin;

  const TelaCarrinho({super.key, required this.aoFinalizar, required this.aoPrecisarLogin});

  @override
  State<TelaCarrinho> createState() => _TelaCarrinhoState();
}

class _TelaCarrinhoState extends State<TelaCarrinho> {
  static const double taxaEntrega = 5.0;
  final enderecoController = TextEditingController();
  String pagamento = 'PIX';
  bool enviando = false;
  String? erro;

  Future<void> finalizar() async {
    final sessao = Sessao.instancia;
    if (!sessao.logado) {
      widget.aoPrecisarLogin();
      return;
    }
    setState(() {
      enviando = true;
      erro = null;
    });
    try {
      await Api.criarPedido(enderecoController.text.trim(), pagamento);
      sessao.limparCarrinho();
      widget.aoFinalizar();
    } catch (e) {
      erro = e.toString().replaceFirst('Exception: ', '');
    }
    setState(() => enviando = false);
  }

  @override
  Widget build(BuildContext context) {
    return ListenableBuilder(
      listenable: Sessao.instancia,
      builder: (context, _) {
        final sessao = Sessao.instancia;
        return Scaffold(
          appBar: AppBar(title: const Text('Carrinho')),
          body: sessao.carrinho.isEmpty
              ? const Center(child: Text('Seu carrinho esta vazio.'))
              : ListView(
                  padding: const EdgeInsets.all(12),
                  children: [
                    for (final item in sessao.carrinho)
                      Card(
                        child: ListTile(
                          title: Text(item.produto.nome),
                          subtitle: Text(moeda(item.produto.preco)),
                          trailing: Row(
                            mainAxisSize: MainAxisSize.min,
                            children: [
                              IconButton(
                                tooltip: 'Diminuir',
                                icon: const Icon(Icons.remove_circle_outline),
                                onPressed: () => sessao.alterar(item, -1),
                              ),
                              Text('${item.quantidade}'),
                              IconButton(
                                tooltip: 'Aumentar',
                                icon: const Icon(Icons.add_circle_outline),
                                onPressed: () => sessao.alterar(item, 1),
                              ),
                            ],
                          ),
                        ),
                      ),
                    const SizedBox(height: 8),
                    Text('Subtotal: ${moeda(sessao.subtotal)}'),
                    Text('Taxa de entrega: ${moeda(taxaEntrega)}'),
                    Text(
                      'Total: ${moeda(sessao.subtotal + taxaEntrega)}',
                      style: Theme.of(context).textTheme.titleLarge,
                    ),
                    const SizedBox(height: 12),
                    TextField(
                      controller: enderecoController,
                      decoration: const InputDecoration(labelText: 'Endereco de entrega'),
                    ),
                    const SizedBox(height: 12),
                    DropdownButtonFormField<String>(
                      initialValue: pagamento,
                      decoration: const InputDecoration(labelText: 'Forma de pagamento'),
                      items: const [
                        DropdownMenuItem(value: 'PIX', child: Text('PIX')),
                        DropdownMenuItem(value: 'CartaoCredito', child: Text('Cartao de credito')),
                        DropdownMenuItem(value: 'CartaoDebito', child: Text('Cartao de debito')),
                        DropdownMenuItem(value: 'Dinheiro', child: Text('Dinheiro')),
                      ],
                      onChanged: (valor) => setState(() => pagamento = valor!),
                    ),
                    if (erro != null)
                      Padding(
                        padding: const EdgeInsets.only(top: 8),
                        child: Text(erro!, style: const TextStyle(color: Color(0xFFB02A2A))),
                      ),
                    const SizedBox(height: 16),
                    FilledButton(
                      onPressed: enviando ? null : finalizar,
                      child: const Text('Finalizar pedido'),
                    ),
                  ],
                ),
        );
      },
    );
  }
}
