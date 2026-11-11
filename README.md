# Rappy Fast Food

Projeto acadêmico de plataforma de pedidos de refeições, desenvolvido no contexto do Projeto Integrado Multidisciplinar (PIM) de Análise e Desenvolvimento de Sistemas.

## Estrutura
- `web/` — interface Web em Blazor / .NET 8.
- `mobile/` — interface Flutter / Dart, publicada também como Flutter Web.
- `api/` — API REST em ASP.NET Core 8.
- `db/` — scripts SQL Server para estrutura, programação e dados iniciais.
- `docker-compose.yml` — orquestração dos serviços.

## Tecnologias
C# • ASP.NET Core 8 • Blazor • Flutter • Dart • SQL Server 2022 • Entity Framework Core • Docker Compose

## Pré-requisitos
- Windows 10/11, Linux ou macOS compatível com Docker Desktop/Engine e Docker Compose.
- Docker em execução. No Windows, use Docker Desktop com mecanismo Linux/WSL 2.
- Conexão com a internet na primeira execução para baixar imagens e dependências.

## Configuração e execução
1. Clone ou extraia o repositório e abra um terminal na pasta que contém `docker-compose.yml`.
2. Crie o arquivo local `.env` a partir do modelo: no PowerShell, `Copy-Item .env.example .env`; no Prompt de Comando, `copy .env.example .env`.
3. Edite `.env` e defina valores próprios e fortes para `MSSQL_SA_PASSWORD` e `JWT_SECRET`. Não publique o `.env`.
4. Para disponibilizar um administrador de demonstração, preencha também `DEMO_ADMIN_EMAIL` e `DEMO_ADMIN_PASSWORD` com credenciais próprias. A senha deve ter pelo menos 6 caracteres. Se deixar esses dois campos vazios, nenhuma conta admin será criada automaticamente.
5. Execute `docker compose up -d --build`.
6. Confira os serviços com `docker compose ps -a`. O esperado é `db` como `healthy`, `db-init` como `Exited (0)` e `api`, `web` e `mobile` como `Up`.

## Contas de demonstração
- **Administrador:** é criado/atualizado pelo `api/Data/DbSeeder.cs` somente quando `DEMO_ADMIN_EMAIL` e `DEMO_ADMIN_PASSWORD` estão configurados no `.env`. A senha não fica gravada no código nem neste repositório.
- **Cliente:** crie pela opção Cliente na página `/cadastro` (ou pelo botão de cadastro no site/app) e use os dados que você definiu.
- **Empresa:** crie pela opção Empresa na página `/cadastro` e use os dados que você definiu.
- As contas antigas de demonstração (`admin@rappy.com`, `cliente@rappy.com` e `empresa@rappy.com`) são desativadas pelo inicializador. Seus registros relacionados não são apagados.

Se alterar as credenciais administrativas no `.env`, recrie a API para aplicar: `docker compose up -d --build api`.

## Endereços locais
- Site Web: http://localhost:8081
- Aplicativo Flutter Web: http://localhost:8082
- API (exemplo de rota): http://localhost:8080/api/produtos
- SQL Server: `localhost,1433` (usuário `sa`; senha definida em `MSSQL_SA_PASSWORD`).

## Parar e preservar os dados
- Parar os contêineres: `docker compose stop`.
- Iniciar novamente: `docker compose up -d`.
- Remover contêineres e rede, mantendo o volume do banco: `docker compose down`.
- **Atenção:** `docker compose down -v` remove também o volume persistente `dbdata` e pode apagar os dados locais. Não use se precisar preservar o banco.

## Observação sobre o volume SQL Server existente
O SQL Server mantém a senha `sa` armazenada no volume persistente quando o banco é inicializado pela primeira vez. Alterar `MSSQL_SA_PASSWORD` no `.env` não altera automaticamente a senha já gravada no volume. Em uma instalação existente, mantenha o `.env` alinhado à senha atual do banco ou altere a senha do login `sa` dentro do SQL Server antes de reiniciar os serviços. Não remova o volume para resolver divergência de senha se os dados precisarem ser preservados.

## Segurança para publicação
- `.env` contém credenciais locais e não deve ser versionado; `.gitignore` já o exclui.
- `.env.example` contém apenas marcadores de configuração, não credenciais válidas de uso compartilhado.
- Não publique dados reais de clientes, tokens, senhas ou credenciais.
- `mobile/android/local.properties` é específico de cada máquina e deve ser gerado no ambiente de desenvolvimento.

## Limites da validação
A preparação dos arquivos não equivale a uma validação de produção. Teste os fluxos de cadastro, login, pedidos, painel administrativo e persistência no ambiente local antes da apresentação.
