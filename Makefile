DB_SERVICE ?= db

-include .env

up:
	docker compose up -d

down:
	docker compose down

restart:
	docker compose down
	docker compose up -d

reset:
	docker compose down -v
	docker compose up -d

psql:
	docker compose exec $(DB_SERVICE) psql -U $(POSTGRES_USER) -d $(POSTGRES_DB)

run-api:
	dotnet run --project backend/FinanceTracker.Api

run: up run-api
