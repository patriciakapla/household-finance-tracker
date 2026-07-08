# ADR 0001 - ESTRUTURA DO PROJETO, DECISÕES ARQUITETURAIS E DESIGN DO BANCO DE DADOS

## ESTRUTURA DO PROJETO

O projeto segue uma arquitetura em camadas, separando responsabilidades entre controllers, services, repositories e camada de acesso a dados. Para a organização física dos arquivos, optei por utilizar *Feature Folders*, agrupando os arquivos relacionados a cada domínio da aplicação em uma mesma pasta.

Essa estrutura facilita a manutenção e a evolução do projeto, pois cada funcionalidade concentra seus models, DTOs, services, repositories e controllers no mesmo contexto.


```txt
backend/
└── FinanceTracker.Api/
    ├── Features/
    │   ├── Users/
    │   └── Transactions/
    ├── Data/
    │   ├── IDbConnectionFactory.cs
    │   └── DbConnectionFactory.cs
    └── Program.cs

database/
└── init.sql
```

## ORGANIZAÇÃO DOS ENDPOINTS POR FEATURE

Dentro de cada Feature Folder, optei por manter um controller e um repository por
recurso principal.

No caso de `Users`, os endpoints de criação, listagem e remoção se concentram em `UsersController`, e o acesso aos dados em `UsersRepository`.

Essa decisão foi tomada porque o escopo atual da feature é pequeno, contendo apenas três operações principais. Separar cada endpoint em subpastas próprias neste momento adicionaria complexidade desnecessária.

Caso a feature cresça ou passe a ter regras mais complexas, a estrutura poderá ser
refinada para separar os casos de uso em arquivos ou pastas específicas.


## BANCO DE DADOS

**POSTGRESQL**

Escolhi PostgreSQL por ser um banco relacional robusto e adequado para projetos financeiros que exigem integridade dos dados. A escolha também permite utilizar recursos como UUID, ENUM, índices e chaves estrangeiras de forma explícita.

### TABELAS

Escolhi *UUID* como chave primária pensando em escalabilidade e maior flexibilidade para futuras integrações e arquiteturas distribuídas, sem depender de sequências incrementais.

#### USER

Criei a coluna booleana *active* para implementar soft delete do registro, dessa maneira os dados são preservados sem que sejam retornados nas consultas da aplicação.

#### TRANSACTION

Coluna *amount* de tipo *decimal*, para maior precisão nos cálculos.

Coluna *type* é um *ENUM*, já que os únicos valores possíveis são "revenue" and "expense".

Optei por indexar o campo *created_at* para que seja utilizado nas queries em que o resultado precisa ser exibido em ordem cronológica, já que a PK é um *UUID* e não representa a ordem de criação dos registros.

## DOCKER
O docker-compose.yml foi utilizado para provisionar o banco PostgreSQL em ambiente local de desenvolvimento.


## USO DE ASYNC/AWAIT

Optei por utilizar métodos assíncronos nas operações que acessam o banco de dados. O uso de `async/await` evita bloquear a thread da requisição enquanto a resposta do banco é aguardada, melhorando a capacidade de lidar com requisições simultâneas.


## Repository Interface

Criei `IUsersRepository` para desacoplar o controller da implementação real e permitir testes do controller sem depender do banco de dados.