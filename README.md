# Cash Manager — Backend

Sistema de controle financeiro pessoal desenvolvido em **C# com ASP.NET Minimal API** e **PostgreSQL**.

---

## Sobre o projeto

O Cash Manager é uma API REST para gerenciamento de finanças pessoais. O sistema permite cadastrar contas, categorias e operações financeiras, com autenticação segura via JWT e hash de senhas com BCrypt.

---

## Tecnologias utilizadas

| Tecnologia | Função |
|---|---|
| C# / ASP.NET Minimal API | Backend e rotas da API |
| PostgreSQL | Banco de dados relacional |
| Npgsql | Conexão C# com PostgreSQL |
| BCrypt.Net | Hash seguro de senhas |
| JWT (JSON Web Token) | Autenticação e autorização |
| DotNetEnv | Leitura de variáveis de ambiente |

---

## Modelagem do banco de dados

```
USUARIO
  └── CONTA
        └── OPERACAO ──── CATEGORIA
```

### Tabelas

- **usuario** — dados de autenticação do usuário
- **conta** — contas financeiras (ex: carteira, Nubank, poupança)
- **categoria** — categorias de entrada ou saída (ex: alimentação, salário)
- **operacao** — registro de entradas e saídas vinculadas a contas e categorias

---

## Autenticação

O sistema utiliza **JWT** para proteger as rotas. O fluxo é:

```
POST /login (email + senha)
        ↓
Backend valida a senha com BCrypt
        ↓
Gera um token JWT com id do usuário
        ↓
Cliente usa o token em todas as requisições seguintes
```

### Rotas públicas (sem token)

| Método | Rota | Descrição |
|---|---|---|
| POST | `/usuario` | Cadastro de novo usuário |
| POST | `/login` | Login e geração do token |

### Rotas protegidas (requerem token)

| Método | Rota | Descrição |
|---|---|---|
| GET | `/contas` | Listar contas do usuário |
| POST | `/conta` | Criar nova conta |
| DELETE | `/conta/{id}` | Deletar conta |
| GET | `/categorias` | Listar categorias do usuário |
| POST | `/categoria` | Criar nova categoria |
| DELETE | `/categoria/{id}` | Deletar categoria |
| GET | `/operacoes` | Listar operações do usuário |
| POST | `/operacao` | Registrar nova operação |
| DELETE | `/operacao/{id}` | Deletar operação |

---

## Estrutura do projeto

```
cash_manager/
├── Controllers/        — recebe as requisições e mapeia as rotas
├── Services/           — regras de negócio e acesso ao banco
├── Models/             — estrutura dos dados
├── Database/           — conexão com o PostgreSQL
├── scripts/            — scripts SQL para criar tabelas e procedures
├── .env.example        — modelo do arquivo de configuração
├── Program.cs          — entrada da aplicação
└── cash_manager.csproj
```

---

## Como rodar localmente

### Pré-requisitos

- [.NET SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)

### Passo a passo

**1. Clone o repositório**
```bash
git clone https://github.com/GITGUSH/cash-manager.git
cd cash-manager
```

**2. Instale os pacotes**
```bash
dotnet restore
```

**3. Configure o banco de dados**

Crie um banco de dados no PostgreSQL e execute os scripts na ordem:
```
scripts/01_tabelas.sql
scripts/02_procedures.sql
```

**4. Configure o .env**

Crie um arquivo `.env` na raiz do projeto baseado no `.env.example`:
```env
DB_HOST=localhost
DB_PORT=5432
DB_NAME=nome_do_banco
DB_USER=postgres
DB_PASSWORD=sua_senha
JWT_SECRET=sua_chave_secreta_jwt
```

**5. Rode o projeto**
```bash
dotnet run
```

A API estará disponível em `http://localhost:5284`.

---

## Como testar as rotas

Use o [Thunder Client](https://www.thunderclient.com/), [Postman](https://www.postman.com/) ou qualquer cliente HTTP.

### Exemplo — Cadastro de usuário
```http
POST /usuario
Content-Type: application/json

{
    "nome": "Gustavo",
    "email": "gustavo@email.com",
    "senhaHash": "sua_senha"
}
```

### Exemplo — Login
```http
POST /login
Content-Type: application/json

{
    "email": "gustavo@email.com",
    "senha": "sua_senha"
}
```

Retorna:
```json
{
    "token": "eyJhbGci..."
}
```

### Exemplo — Criar conta (com token)
```http
POST /conta
Authorization: Bearer eyJhbGci...
Content-Type: application/json

{
    "nome": "Nubank",
    "saldo": 800.00
}
```

---

## Segurança

- Senhas armazenadas com hash BCrypt — nunca em texto puro
- Rotas protegidas por JWT — sem token, sem acesso
- Cada usuário acessa apenas seus próprios dados
- Credenciais nunca sobem para o repositório via `.env`

---

## Autor

Desenvolvido por **Gustavo Fiocco**

[![GitHub](https://img.shields.io/badge/GitHub-GITGUSH-181717?style=flat&logo=github)](https://github.com/GITGUSH)