# 🐳 Docker Setup - SGF

Este guia explica como executar o projeto SGF usando Docker, sem necessidade de configurar o ambiente local.

> 📖 **Para informações gerais sobre o projeto, consulte o [README principal](README.md)**

## 📋 Pré-requisitos

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado e em execução
- Portas **1433** (SQL Server) e **5000** (API) disponíveis

## 🚀 Como executar

### 1. Executar com Docker Compose (Recomendado)

Na raiz do projeto, execute:

```bash
docker-compose up -d
```

Este comando irá:
- ✅ Baixar a imagem do SQL Server 2025
- ✅ Construir a imagem da API
- ✅ Criar uma rede Docker para comunicação entre os serviços
- ✅ Criar um volume persistente para o banco de dados
- ✅ Aguardar o SQL Server estar saudável antes de iniciar a API
- ✅ Aplicar as migrations automaticamente na inicialização da API

### 2. Acessar a aplicação

- **API**: http://localhost:5000
- **Swagger/Scalar**: http://localhost:5000/scalar/v1
- **SQL Server**: `localhost,1433`
  - **Usuário**: `sa`
  - **Senha**: `SGFPass@Strong123`
  - **Database**: `SGFDatabase`

## 🛠️ Comandos úteis

### Ver logs dos containers
```bash
docker-compose logs -f
```

### Ver logs apenas da API
```bash
docker-compose logs -f api
```

### Ver logs apenas do SQL Server
```bash
docker-compose logs -f sqlserver
```

### Parar os containers
```bash
docker-compose down
```

### Parar e remover volumes (⚠️ apaga os dados do banco)
```bash
docker-compose down -v
```

### Reconstruir a imagem da API
```bash
docker-compose build api
docker-compose up -d api
```

### Executar comandos dentro do container da API
```bash
docker-compose exec api bash
```

### Conectar ao SQL Server via sqlcmd
```bash
docker-compose exec sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "SGFPass@Strong123" -C
```

## 🔧 Troubleshooting

### Porta já em uso
Se as portas 1433 ou 5000 já estiverem em uso, você pode alterá-las no `docker-compose.yml`:

```yaml
services:
  sqlserver:
    ports:
      - "1434:1433"  # Muda a porta local para 1434
  
  api:
    ports:
      - "5001:8080"  # Muda a porta local para 5001
```

### SQL Server não inicia
Verifique se o Docker Desktop tem recursos suficientes alocados (mínimo 2GB de RAM).

### Migrations não aplicadas automaticamente
As migrations precisam ser executadas manualmente após os containers subirem. Use o comando da seção 2.

## 📦 Estrutura

- **Dockerfile**: Define como construir a imagem da API
- **docker-compose.yml**: Orquestra a API e o SQL Server
- **.dockerignore**: Otimiza o build excluindo arquivos desnecessários
- **appsettings.Docker.json**: Configurações específicas para ambiente Docker

## 🔐 Segurança

**⚠️ IMPORTANTE**: As credenciais neste setup são apenas para desenvolvimento local. 

Para produção:
- Use variáveis de ambiente ou secrets do Docker
- Altere as senhas
- Configure certificados SSL/TLS adequados
- Utilize um provedor de gerenciamento de secrets (Azure Key Vault, AWS Secrets Service, etc.)
