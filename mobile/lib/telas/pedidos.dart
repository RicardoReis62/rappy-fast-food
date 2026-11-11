import 'dart:async';

import 'package:flutter/material.dart';

import '../api.dart';
import '../modelos.dart';
import '../sessao.dart';

class TelaPedidos extends StatefulWidget {
  const TelaPedidos({super.key});

  @override
  State<TelaPedidos> createState() => _TelaPedidosState();
}

class _TelaPedidosState extends State<TelaPedidos> {
  List<Pedido> pedidos = [];
  bool carregando = false;
  String? erro;
  Timer? temporizador;

  @override
  void initState() {
    super.initState();
    Sessao.instancia.addListener(carregar);
    carregar();
    temporizador = Timer.periodic(const Duration(seconds: 15), (_) => carregar());
  }

  @override
  void dispose() {
    temporizador?.cancel();
    Sessao.instancia.removeListener(carregar);
    super.dispose();
  }

  Future<void> carregar() async {
    if (!Sessao.instancia.logado) {
      if (mounted) setState(() => pedidos = []);
      return;
    }
    setState(() => carregando = true);
    try {
      final lista = await Api.meusPedidos();
      if (mounted) setState(() => pedidos = lista);
      erro = null;
    } catch (e) {
      erro = 'Nao foi possivel carregar os pedidos.';
    }
    if (mounted) setState(() => carregando = false);
  }

  String rotulo(String status) {
    return status == 'SaiuParaEntrega' ? 'Saiu para entrega' : status;
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Meus pedidos')),
      body: !Sessao.instancia.logado
          ? const Center(child: Text('Entre na sua conta para ver os pedidos.'))
          : erro != null
              ? Center(child: Text(erro!))
              : RefreshIndicator(
                  onRefresh: carregar,
                  child: pedidos.isEmpty && !carregando
                      ? ListView(children: const [SizedBox(height: 120), Center(child: Text('Nenhum pedido ainda.'))])
                      : ListView.builder(
                          itemCount: pedidos.length,
                          itemBuilder: (context, indice) {
                            final pedido = pedidos[indice];
                            return Card(
                              margin: const EdgeInsets.symmetric(horizontal: 12, vertical: 6),
                              child: ListTile(
                                title: Text('Pedido #${pedido.id} - ${rotulo(pedido.status)}'),
                                subtitle: Text(
                                  pedido.itens.map((i) => '${i.quantidade}x ${i.produto}').join('\n'),
                                ),
                                trailing: Text(moeda(pedido.valorTotal)),
                                isThreeLine: pedido.itens.length > 1,
                              ),
                            );
                          },
                        ),
                ),
    );
  }
}
