# Finance Tracker Backend

API para controle de gastos residenciais desenvolvida em .NET.

- [Decisões arquiteturais](./adr.md)
- [Repositório do frontend](https://github.com/patriciakapla/finance-tracker-frontend)

## Requisitos

- .NET 8 SDK
- Docker

## Configuração do banco local

O banco PostgreSQL é executado via Docker Compose.

Crie o arquivo `.env` a partir do exemplo:

```bash
cp .env.example .env
```

Suba o banco:

```bash
docker compose up -d
```

## Configuração da connection string da API

O projeto não versiona a connection string real com senha.

A connection string local deve ser configurada com `dotnet user-secrets`:

```bash
dotnet user-secrets set \
  "ConnectionStrings:DefaultConnection" \
  "Host=localhost;Port=5433;Database=finance_tracker;Username=finance_tracker;Password=finance_tracker_password" \
  --project backend/FinanceTracker.Api
```

Use o mesmo valor definido em `POSTGRES_PASSWORD` no arquivo `.env`.

## Executando a API

```bash
dotnet run --project backend/FinanceTracker.Api
```