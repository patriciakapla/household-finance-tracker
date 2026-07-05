# Household Finance Tracker

API para controle de gastos residenciais desenvolvida em .NET.

## Requisitos

- .NET 8 SDK
- Docker
- Docker Compose

## Configuração do banco local

O banco PostgreSQL é executado via Docker Compose.

Crie o arquivo `.env` a partir do exemplo:

```bash
cp .env.example .env
```

Edite o valor de `POSTGRES_PASSWORD` no arquivo `.env`, se necessário.

Suba o banco:

```bash
docker compose up -d
```

Ou usando o Makefile:

```bash
make up
```

## Configuração da connection string da API

O projeto não versiona a connection string real com senha.

A connection string local deve ser configurada com `dotnet user-secrets`:

```bash
dotnet user-secrets set \
  "ConnectionStrings:DefaultConnection" \
  "Host=localhost;Port=5433;Database=finance_tracker;Username=finance_tracker;Password=change_me" \
  --project backend/FinanceTracker.Api
```3

Use o mesmo valor definido em `POSTGRES_PASSWORD` no arquivo `.env`.

## Executando a API

```bash
dotnet run --project backend/FinanceTracker.Api
```

Com a aplicação em ambiente de desenvolvimento, a documentação Swagger fica disponível na URL exibida pelo terminal.

## Comandos úteis

```bash
make up
make down
make restart
make reset
make psql
```

