# Rappy Fast Food

Projeto acadêmico de plataforma de pedidos de refeições, desenvolvido no contexto do Projeto Integrado Multidisciplinar (PIM) de Análise e Desenvolvimento de Sistemas.

## Estrutura
- `web/` — interface Web em Blazor / .NET 8.
- `mobile/` — interface Flutter / Dart (inclui publicação Flutter Web).
- `api/` — API REST em ASP.NET Core 8.
- `db/` — scripts SQL Server para estrutura, programação e dados iniciais.
- `docker-compose.yml` — orquestração dos serviços.

## Tecnologias
C# • ASP.NET Core 8 • Blazor • Flutter • Dart • SQL Server 2022 • Entity Framework Core • Docker Compose

## Pré-requisitos
- Docker Desktop com Docker Compose.

## Configuração e execução
1. Clone o repositório e entre na pasta `rappy-fast-food`.
2. Copie `.env.example` para `.env` (`cp .env.example .env`; no PowerShell: `Copy-Item .env.example .env`).
3. Edite `.env` e defina uma senha forte para `MSSQL_SA_PASSWORD` e uma chave aleatória forte para `JWT_SECRET`. Nunca envie `.env` ao GitHub.
4. Execute `docker compose up --build`.

## Endereços locais
- Site Web: http://localhost:8081
- Aplicativo Flutter Web: http://localhost:8082
- API: http://localhost:8080
- SQL Server: `localhost,1433`

Para encerrar: `docker compose down`. Atenção: `docker compose down -v` também remove o volume persistente do banco e pode apagar os dados locais.

## Segurança para publicação
- O arquivo `.env` contém credenciais locais e não deve ser versionado.
- Use valores próprios e fortes no `.env`; os valores de `.env.example` são apenas exemplos.
- Não publique dados reais de clientes, tokens, senhas ou credenciais.
- `mobile/android/local.properties` foi excluído por conter caminhos específicos da máquina; o Flutter/Android pode regenerá-lo no ambiente de cada desenvolvedor.

## Observação
A preparação para o GitHub não comprova compilação, execução ou validação em produção. Execute e teste os fluxos no ambiente local antes de apresentar o sistema como operacional.
