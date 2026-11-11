import 'package:flutter/material.dart';

import 'sessao.dart';
import 'telas/cardapio.dart';
import 'telas/carrinho.dart';
import 'telas/conta.dart';
import 'telas/pedidos.dart';

void main() {
  runApp(const RappyApp());
}

class RappyApp extends StatelessWidget {
  const RappyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Rappy Fast Food',
      debugShowCheckedModeBanner: false,
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(
          seedColor: const Color(0xFFD63C3C),
          primary: const Color(0xFFD63C3C),
        ),
        appBarTheme: const AppBarTheme(
          backgroundColor: Color(0xFF2C3E50),
          foregroundColor: Colors.white,
        ),
        useMaterial3: true,
      ),
      home: const Raiz(),
    );
  }
}

class Raiz extends StatefulWidget {
  const Raiz({super.key});

  @override
  State<Raiz> createState() => _RaizState();
}

class _RaizState extends State<Raiz> {
  int aba = 0;

  @override
  Widget build(BuildContext context) {
    final telas = [
      const TelaCardapio(),
      TelaCarrinho(
        aoFinalizar: () => setState(() => aba = 2),
        aoPrecisarLogin: () => setState(() => aba = 3),
      ),
      const TelaPedidos(),
      const TelaConta(),
    ];

    return ListenableBuilder(
      listenable: Sessao.instancia,
      builder: (context, _) {
        return Scaffold(
          body: IndexedStack(index: aba, children: telas),
          bottomNavigationBar: NavigationBar(
            selectedIndex: aba,
            onDestinationSelected: (indice) => setState(() => aba = indice),
            destinations: [
              const NavigationDestination(icon: Icon(Icons.restaurant_menu), label: 'Cardapio'),
              NavigationDestination(
                icon: Badge(
                  isLabelVisible: Sessao.instancia.totalItens > 0,
                  label: Text('${Sessao.instancia.totalItens}'),
                  child: const Icon(Icons.shopping_cart),
                ),
                label: 'Carrinho',
              ),
              const NavigationDestination(icon: Icon(Icons.receipt_long), label: 'Pedidos'),
              const NavigationDestination(icon: Icon(Icons.person), label: 'Conta'),
            ],
          ),
        );
      },
    );
  }
}
