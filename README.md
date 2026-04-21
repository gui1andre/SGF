# 📋 SGF - Sistema de Gerenciamento de Faturas

> 🏆 **Desafio Técnico - Backend Pleno .NET | [Scorb](https://scorb.com.br)**

Sistema para gerenciamento de faturas e seus itens, desenvolvido com .NET 10, Entity Framework Core e SQL Server.

## 📑 Índice

- [Sobre o Projeto](#-sobre-o-projeto)
- [Funcionalidades](#-funcionalidades)
- [Arquitetura](#-arquitetura)
- [Tecnologias](#-tecnologias)
- [Pré-requisitos](#-pré-requisitos)
- [Instalação](#-instalação)
- [Executando com Docker](#-executando-com-docker)
- [Estrutura do Projeto](#-estrutura-do-projeto)
- [API Endpoints](#-api-endpoints)
- [Regras de Negócio](#-regras-de-negócio)
- [Contribuindo](#-contribuindo)

## 🎯 Sobre o Projeto

O SGF (Sistema de Gerenciamento de Faturas) é uma aplicação RESTful desenvolvida como **desafio técnico para a vaga de Backend Pleno .NET na empresa Scorb**.

O sistema permite criar, atualizar, listar e fechar faturas, além de gerenciar os itens associados a cada fatura.

## ✨ Funcionalidades

### Gestão de Faturas
- ✅ Criar novas faturas
- ✅ Atualizar dados do cliente
- ✅ Listar faturas com filtros (cliente, data, status)
- ✅ Consultar fatura por ID
- ✅ Fechar faturas
- ✅ Excluir faturas abertas
- ✅ Auditoria (data de criação e atualização)

### Gestão de Itens
- ✅ Adicionar itens à fatura
- ✅ Atualizar itens da fatura
- ✅ Remover itens da fatura
- ✅ Cálculo automático do valor total
- ✅ Validação de justificativa para itens acima de R$ 1.000,00

## 🏗 Arquitetura

O projeto segue os princípios de **Clean Architecture** e **Domain-Driven Design (DDD)**, organizado em camadas:

```
SGF/
├── Domain/              # Camada de Domínio (Entidades, Regras de Negócio)
├── Application/         # Camada de Aplicação (Casos de Uso, DTOs)
├── Infrastructure/      # Camada de Infraestrutura (Repositórios, EF Core)
└── SGF.API/            # Camada de Apresentação (Controllers, Middleware)
```

### Camadas

#### 🔵 Domain
- Entidades: `Fatura`, `ItemFatura`, `BaseEntity`
- Enums: `StatusFatura` (Aberta, Fechada)
- Interfaces de repositório
- Regras de negócio e validações de domínio

#### 🟢 Application
- Services: `FaturaService`
- Interface: `IFaturaService`
- DTOs (Data Transfer Objects)
- Validadores (FluentValidation)

#### 🟡 Infrastructure
- Implementação de repositórios
- Context do Entity Framework Core
- Migrations do banco de dados
- Configurações de persistência

#### 🔴 SGF.API
- Controllers RESTful
- Middleware de tratamento de exceções
- Configuração de Dependency Injection
- Documentação OpenAPI/Scalar

## 🛠 Tecnologias

- **.NET 10** - Framework principal
- **ASP.NET Core** - Web API
- **Entity Framework Core 10** - ORM
- **SQL Server 2025** - Banco de dados
- **FluentValidation** - Validação de DTOs
- **Scalar** - Documentação interativa da API
- **Docker & Docker Compose** - Containerização

## 📋 Pré-requisitos

### Opção 1: Desenvolvimento Local
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server 2025](https://www.microsoft.com/sql-server) ou SQL Server Express
- [Visual Studio 2026](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)

### Opção 2: Docker (Recomendado)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

## 🚀 Instalação

### Desenvolvimento Local

1. **Clone o repositório**
```bash
git clone https://github.com/gui1andre/SGF.git
cd SGF
```

2. **Configure a string de conexão**

Edite o arquivo `SGF.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=SGFDatabase;User Id=sa;Password=SuaSenha;TrustServerCertificate=True"
  }
}
```

3. **Restaure os pacotes**
```bash
dotnet restore
```

4. **Execute as migrations**
```bash
dotnet ef database update --project Infrastructure/Infrastructure.csproj --startup-project SGF.API/SGF.API.csproj
```

5. **Execute a aplicação**
```bash
cd SGF.API
dotnet run
```

A API estará disponível em:
- HTTP: `http://localhost:5000`
- Documentação: `http://localhost:5000/scalar/v1`

## 🐳 Executando com Docker

Para executar o projeto completo com Docker (incluindo SQL Server), consulte a documentação detalhada:

**[📖 Guia Docker Completo →](README.Docker.md)**

Resumo rápido:
```bash
# Iniciar containers (migrations aplicadas automaticamente na inicialização)
docker-compose up -d

# Acessar a API
# http://localhost:5000
```

## 📁 Estrutura do Projeto

```
SGF/
│
├── Domain/
│   ├── Common/
│   │   └── BaseEntity.cs                  # Entidade base com Id, CriadaEm, AtualizadaEm
│   ├── Faturas/
│   │   ├── Entities/
│   │   │   └── Fatura.cs                  # Entidade Fatura
│   │   ├── Enums/
│   │   │   └── StatusFatura.cs            # Enum de Status
│   │   └── Interfaces/
│   │       └── IFaturaRepository.cs        # Interface do repositório
│   └── ItensFatura/
│       ├── Entities/
│       │   └── ItemFatura.cs              # Entidade ItemFatura
│       └── Interfaces/
│           └── IItemFaturaRepository.cs    # Interface do repositório
│
├── Application/
│   └── Faturas/
│       ├── DTO/                           # Data Transfer Objects
│       ├── Ports/
│       │   └── IFaturaService.cs          # Interface do gerenciador
│       ├── Validators/                    # Validadores FluentValidation
│       └── FaturaService.cs               # Lógica de aplicação
│
├── Infrastructure/
│   ├── Data/
│   │   └── AppDBContext.cs                # Contexto do EF Core
│   ├── Repositories/                      # Implementações dos repositórios
│   └── Migrations/                        # Migrations do EF Core
│
├── SGF.API/
│   ├── Controllers/
│   │   └── FaturaController.cs            # Endpoints REST
│   ├── Middleware/
│   │   └── ExceptionHadnleMiddleware.cs   # Tratamento global de exceções
│   ├── Program.cs                         # Configuração da aplicação
│   ├── appsettings.json                   # Configurações
│   └── appsettings.Docker.json            # Configurações para ambiente Docker
│
├── Dockerfile                             # Imagem Docker da API
├── docker-compose.yml                     # Orquestração Docker
├── .dockerignore                          # Exclusões do build Docker
├── README.Docker.md                       # Guia de execução com Docker
└── README.md                              # Este arquivo
```

## 🌐 API Endpoints

### Faturas

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `POST` | `/api/fatura` | Criar nova fatura |
| `GET` | `/api/fatura/{id}` | Obter fatura por ID |
| `GET` | `/api/fatura` | Listar faturas (com filtros) |
| `PUT` | `/api/fatura/{id}` | Atualizar cliente da fatura |
| `PUT` | `/api/fatura/{id}/fechar` | Fechar fatura |
| `DELETE` | `/api/fatura/{id}` | Excluir fatura (apenas abertas) |

### Itens da Fatura

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `POST` | `/api/fatura/{faturaId}/itens` | Adicionar item à fatura |
| `PUT` | `/api/fatura/{faturaId}/itens/{itemId}` | Atualizar item da fatura |
| `DELETE` | `/api/fatura/{faturaId}/itens/{itemId}` | Remover item da fatura |

### Exemplos de Requisições

#### Criar Fatura
```http
POST /api/fatura
Content-Type: application/json

{
  "nomeCliente": "João Silva"
}
```

#### Adicionar Item
```http
POST /api/fatura/{faturaId}/itens
Content-Type: application/json

{
  "descricao": "Notebook Dell",
  "quantidade": 1,
  "valorUnitario": 3500.00,
  "justificativa": "Equipamento para desenvolvimento"
}
```

#### Listar Faturas com Filtros
```http
GET /api/fatura?nomeCliente=João&status=Aberta&dataInicial=2026-01-01&dataFinal=2026-12-31
```

## 📜 Regras de Negócio

### Faturas
- ✅ Uma fatura inicia com status **Aberta**
- ✅ Apenas faturas **Abertas** podem ser alteradas
- ✅ Para fechar uma fatura, ela deve ter **pelo menos 1 item**
- ✅ Faturas **Fechadas** recebem a data de emissão automaticamente
- ✅ Faturas **Fechadas** não podem ser editadas ou excluídas
- ✅ O valor total é **calculado automaticamente** com base nos itens

### Itens da Fatura
- ✅ Descrição deve ter **no mínimo 5 caracteres**
- ✅ Quantidade deve ser **maior que 0**
- ✅ Valor unitário deve ser **maior que 0**
- ✅ Itens com valor total **acima de R$ 1.000,00** exigem **justificativa**
- ✅ O valor total do item é **calculado automaticamente** (quantidade × valor unitário)

### Auditoria
- ✅ Todas as entidades possuem `CriadaEm` (preenchida automaticamente na criação)
- ✅ Todas as entidades possuem `AtualizadaEm` (atualizada em cada modificação)

## 🧪 Testes

```bash
# Executar todos os testes
dotnet test

# Executar testes com cobertura
dotnet test /p:CollectCoverage=true
```

## 🤝 Contribuindo

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona MinhaFeature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

## 📝 Licença

Este projeto está sob a licença MIT.

## 👤 Autor

**Guilherme André**
- GitHub: [@gui1andre](https://github.com/gui1andre)

---

⭐ **Se este projeto foi útil, considere dar uma estrela no repositório!**
