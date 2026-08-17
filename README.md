# 🏦 BankKRT — API de Gerenciamento de Contas

API RESTful para gerenciamento de contas do **Banco KRT**, desenvolvida como teste técnico para o time de Onboarding.

## 📋 Sobre o Projeto

Esta API permite o CRUD completo de contas bancárias, com:

- **Notificação de eventos** — Áreas como Prevenção à Fraude e Cartões são notificadas automaticamente quando contas são criadas, atualizadas ou deletadas
- **Cache inteligente** — Consultas ao banco de dados são cacheadas para reduzir custos com infraestrutura (AWS)
- **Validação de CPF** — Implementada como Value Object com validação completa

## 🏗️ Arquitetura

O projeto segue os princípios de **DDD (Domain-Driven Design)** e **Clean Architecture**:

```
src/
├── BankKRT.API              → Camada de Apresentação (Controllers, Middlewares)
├── BankKRT.Application      → Camada de Aplicação (Services, DTOs, Validators)
├── BankKRT.Domain           → Camada de Domínio (Entities, Value Objects, Events)
└── BankKRT.Infrastructure   → Camada de Infraestrutura (EF Core, Cache, Repositories)

tests/
├── BankKRT.UnitTests        → Testes Unitários
└── BankKRT.IntegrationTests → Testes de Integração
```

## 🛠️ Tecnologias

| Tecnologia | Versão | Finalidade |
|-----------|--------|------------|
| .NET | 8.0 | Framework principal |
| PostgreSQL | 18.x | Banco de dados |
| Entity Framework Core | 8.x | ORM |
| MediatR | 12.x | Domain Events |
| FluentValidation | 11.x | Validação de entrada |
| xUnit | 2.x | Framework de testes |
| Moq | 4.x | Mocking para testes |
| FluentAssertions | 6.x | Assertions legíveis |

## 🚀 Como Executar

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/) (ou Docker)
- [Docker](https://www.docker.com/) (opcional)

### PostgreSQL com Docker

```bash
docker run --name postgres -e POSTGRES_PASSWORD=postgres -p 5432:5432 -d postgres
```

### Executando a API

```bash
# Restaurar dependências
dotnet restore

# Aplicar migrations
dotnet ef database update --project src/BankKRT.Infrastructure --startup-project src/BankKRT.API

# Executar
dotnet run --project src/BankKRT.API
```

A API estará disponível em: `https://localhost:5001` | `http://localhost:5000`

Swagger UI: `https://localhost:5001/swagger`

### Executando os Testes

```bash
# Todos os testes
dotnet test

# Apenas unitários
dotnet test tests/BankKRT.UnitTests

# Com cobertura
dotnet test --collect:"XPlat Code Coverage"
```

## 📡 Endpoints

| Método | Rota | Descrição |
|--------|------|-----------|
| `POST` | `/api/accounts` | Criar nova conta |
| `GET` | `/api/accounts` | Listar todas as contas |
| `GET` | `/api/accounts/{id}` | Buscar conta por ID |
| `PUT` | `/api/accounts/{id}` | Atualizar conta |
| `DELETE` | `/api/accounts/{id}` | Deletar conta |

### Exemplos de Uso

#### Criar Conta
```bash
curl -X POST https://localhost:5001/api/accounts \
  -H "Content-Type: application/json" \
  -d '{
    "holderName": "João da Silva",
    "cpf": "529.982.247-25"
  }'
```

#### Resposta (201 Created)
```json
{
  "id": 1,
  "holderName": "João da Silva",
  "cpf": "52998224725",
  "status": "Active",
  "createdAt": "2026-08-17T08:00:00Z",
  "updatedAt": null
}
```

#### Atualizar Conta
```bash
curl -X PUT https://localhost:5001/api/accounts/1 \
  -H "Content-Type: application/json" \
  -d '{
    "holderName": "João da Silva Santos",
    "status": "Inactive"
  }'
```

## 🧩 Padrões e Princípios

- **SOLID** — Inversão de dependência via interfaces, responsabilidade única nos services
- **Clean Code** — Nomes descritivos, métodos curtos e coesos
- **DDD** — Value Objects (CPF), Domain Events, Repository Pattern
- **MVC** — Controllers magros, lógica nos Services
- **Clean Architecture** — Camadas independentes com fluxo de dependência unidirecional

## 📐 Decisões de Design

### Cache
- Utiliza `IMemoryCache` com expiração até o final do dia
- Invalida automaticamente em operações de escrita
- Reduz consultas desnecessárias ao PostgreSQL

### Domain Events
- Implementados com MediatR (`INotification`)
- Handlers simulam notificações para áreas do banco:
  - `FraudPreventionHandler` — Prevenção à Fraude
  - `CardDepartmentHandler` — Departamento de Cartões
  - `ComplianceHandler` — Compliance

### Validação de CPF
- Implementada como **Value Object** no domínio
- Valida dígitos verificadores conforme algoritmo oficial
- Aceita formatos com ou sem máscara (xxx.xxx.xxx-xx)

## 👤 Autor

**Deryk Mark Kadiama**

---

*Desenvolvido como teste técnico para o BTG Pactual*
