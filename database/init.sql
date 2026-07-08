CREATE EXTENSION IF NOT EXISTS pgcrypto;

CREATE TYPE transaction_type AS ENUM (
    'revenue',
    'expense'
);

CREATE TABLE IF NOT EXISTS users (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "name" VARCHAR(255) NOT NULL,
    birth_date DATE NOT NULL,
    active BOOL NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS transactions (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID NOT NULL,
    "description" VARCHAR(255) NOT NULL,
    amount DECIMAL(8,2) NOT NULL,
    "type" transaction_type NOT NULL,
    created_at TIMESTAMPTZ DEFAULT now(),

    CONSTRAINT fk_transaction_user
    FOREIGN KEY (user_id)
    REFERENCES users(id)
);

CREATE INDEX IF NOT EXISTS idx_transactions_created_at
ON transactions(created_at);