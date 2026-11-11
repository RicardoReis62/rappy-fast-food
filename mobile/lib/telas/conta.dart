import 'package:flutter/material.dart';

import '../api.dart';
import '../sessao.dart';

class TelaConta extends StatefulWidget {
  const TelaConta({super.key});

  @override
  State<TelaConta> createState() => _TelaContaState();
}

class _TelaContaState extends State<TelaConta> {
  final nome = TextEditingController();
  final email = TextEditingController();
  final senha = TextEditingController();
  final telefone = TextEditingController();
  final endereco = TextEditingController();
  bool cadastro = false;
  bool ocultar = true;
  bool enviando = false;
  String? erro;

  Future<void> enviar() async {
    setState(() {
      enviando = true;
      erro = null;
    });
    try {
      final usuario = cadastro
          ? await Api.cadastrar({
              'nome': nome.text.trim(),
              'email': email.text.trim(),
              'senha': senha.text,
              'telefone': telefone.text.trim(),
              'endereco': endereco.text.trim(),
            })
          : await Api.login(email.text.trim(), senha.text);
      Sessao.instancia.entrar(usuario);
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
        final usuario = Sessao.instancia.usuario;
        if (usuario != null) {
          return Scaffold(
            appBar: AppBar(title: const Text('Minha conta')),
            body: Padding(
              padding: const EdgeInsets.all(16),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(usuario.nome, style: Theme.of(context).textTheme.headlineSmall),
                  Text(usuario.email),
                  Text('Perfil: ${usuario.tipo}'),
                  const SizedBox(height: 24),
                  OutlinedButton(onPressed: Sessao.instancia.sair, child: const Text('Sair')),
                ],
              ),
            ),
          );
        }

        return Scaffold(
          appBar: AppBar(title: Text(cadastro ? 'Cadastro' : 'Entrar')),
          body: ListView(
            padding: const EdgeInsets.all(16),
            children: [
              if (cadastro) ...[
                TextField(controller: nome, decoration: const InputDecoration(labelText: 'Nome completo')),
                const SizedBox(height: 12),
              ],
              TextField(
                controller: email,
                keyboardType: TextInputType.emailAddress,
                decoration: const InputDecoration(labelText: 'E-mail'),
              ),
              const SizedBox(height: 12),
              TextField(
                controller: senha,
                obscureText: ocultar,
                decoration: InputDecoration(
                  labelText: 'Senha',
                  suffixIcon: IconButton(
                    tooltip: ocultar ? 'Mostrar senha' : 'Ocultar senha',
                    icon: Icon(ocultar ? Icons.visibility : Icons.visibility_off),
                    onPressed: () => setState(() => ocultar = !ocultar),
                  ),
                ),
              ),
              if (cadastro) ...[
                const SizedBox(height: 12),
                TextField(controller: telefone, decoration: const InputDecoration(labelText: 'Telefone')),
                const SizedBox(height: 12),
                TextField(controller: endereco, decoration: const InputDecoration(labelText: 'Endereco')),
              ],
              if (erro != null)
                Padding(
                  padding: const EdgeInsets.only(top: 12),
                  child: Text(erro!, style: const TextStyle(color: Color(0xFFB02A2A))),
                ),
              const SizedBox(height: 16),
              FilledButton(
                onPressed: enviando ? null : enviar,
                child: Text(cadastro ? 'Cadastrar' : 'Entrar'),
              ),
              TextButton(
                onPressed: () => setState(() {
                  cadastro = !cadastro;
                  erro = null;
                }),
                child: Text(cadastro ? 'Ja tenho conta' : 'Criar conta'),
              ),
            ],
          ),
        );
      },
    );
  }
}
