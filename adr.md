# ADR 

## ADR 01 - ORGANIZAÇÃO DO BACKEND

- O backend é organizado em camadas, utilizando Feature Folders para agrupar os arquivos relacionados a cada domínio, mantendo as features principais (Users e Transactions) separadas.

- Cada feature concentra seus controllers, models e repositories:
Os controllers expõem os endpoints HTTP, enquanto os repositories centralizam o acesso ao banco de dados. 
As interfaces de repository desacoplam os controllers das implementações concretas, facilitando testes e futuras mudanças.

## ADR 02 - API E ENDPOINTS

A API expõe endpoints para usuários, transações e relatórios financeiros, com controllers ASP.NET Core como pontos de entrada HTTP. Em desenvolvimento,  CORS permite o frontend local em http://localhost:5173.

Usuários podem ser criados, listados e removidos por soft delete, mantendo seus registros no banco de dados, porém invisíveis para as consultas da aplicação. 

Transações podem ser criadas e listadas para usuários ativos, respeitando a regra de que menores de 18 anos não podem cadastrar receitas.

Os relatórios são separados entre dados por usuário e total geral, permitindo futura paginação no relatório sem afetar o cálculo dos totais.

## ADR 03 - BANCO DE DADOS

O projeto usa PostgreSQL por oferecer integridade relacional e suporte a UUID, ENUM, índices e chaves estrangeiras. O schema inicial fica em database/init.sql.

A tabela users armazena pessoas cadastradas e usa o campo "active" para soft delete. A tabela transactions armazena receitas e despesas vinculadas a usuários (user_id), com valor decimal para maior precisão e type limitado a revenue ou expense (ENUM).

As transações são ordenadas por created_at, já que UUID não indica ordem de criação. Esse campo é indexado para apoiar listagens cronológicas e relatórios financeiros.

O acesso ao banco é concentrado em UsersRepository e TransactionsRepository, expostos aos controllers por interfaces. Isso desacopla os controllers da implementação real e facilita testes com repositories falsos.

O projeto usa Dapper com Npgsql para acessar PostgreSQL sem adicionar um ORM pesado. O SQL fica explícito nos repositories, mantendo controle sobre consultas,
joins, filtros e agregações. 

A criação de conexões é centralizada em DbConnectionFactory, que lê a connection string DefaultConnection. 

As operações usam async/await, evitando bloquear threads enquanto aguardam respostas do banco.

## ADR 04 - INFRAESTRUTURA E CONFIGURAÇÃO

O projeto usa docker-compose.yml para provisionar PostgreSQL localmente, com schema inicial carregado a partir de database/init.sql. O

As credenciais do banco são lidas por variáveis de ambiente, normalmente via .env, e não devem ser reutilizadas em produção.
