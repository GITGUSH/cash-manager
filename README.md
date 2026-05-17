# Cash Manager — Backend

Sistema de controle financeiro pessoal desenvolvido em **C# com ASP.NET Minimal API** e **PostgreSQL**.

---

## Sobre o projeto

O Cash Manager é uma API REST para gerenciamento de finanças pessoais. O sistema permite cadastrar contas, categorias e operações financeiras, com autenticação segura via JWT, hash de senhas com BCrypt e envio de extratos por email.

---

## Regras de negócio

- Contas e categorias não podem ser deletadas se possuírem operações vinculadas
- Ao deletar uma operação, o saldo da conta é revertido automaticamente
- Emails de usuário são únicos no sistema
- Operações de saída só aceitam categorias do tipo saída e vice-versa
- Extrato de operações por conta filtrável por período com envio automático por email

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
| MailKit | Envio de emails com anexo PDF |

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
| GET | `/usuario/perfil` | Dados do usuário logado |
| GET | `/contas` | Listar contas do usuário |
| POST | `/conta` | Criar nova conta |
| DELETE | `/conta/{id}` | Deletar conta |
| GET | `/categorias` | Listar categorias do usuário |
| POST | `/categoria` | Criar nova categoria |
| DELETE | `/categoria/{id}` | Deletar categoria |
| GET | `/operacoes` | Listar todas as operações do usuário |
| POST | `/operacao` | Registrar nova operação |
| DELETE | `/operacao/{id}` | Deletar operação |
| GET | `/operacoes/conta/{id}` | Listar operações de uma conta por período |
| POST | `/email/extrato` | Enviar extrato PDF por email |
| POST | `/shutdown` | Encerrar o sistema |

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
EMAIL_USUARIO=seuemail@gmail.com
EMAIL_SENHA=sua_senha_de_app_gmail
```

> Para o envio de emails, é necessário gerar uma **Senha de App** no Gmail em [myaccount.google.com](https://myaccount.google.com) → Segurança → Senhas de app.

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

### Exemplo — Extrato por período
```http
GET /operacoes/conta/1?dataInicio=2026-01-01&dataFim=2026-05-17
Authorization: Bearer eyJhbGci...
```

### Exemplo — Enviar extrato por email
```http
POST /email/extrato
Authorization: Bearer eyJhbGci...
Content-Type: application/json

{
    "destinatario": "gustavo@email.com",
    "nomeDestinatario": "Gustavo",
    "assuntoPdf": "Extrato NUBANK — 01/01/2026 até 17/05/2026",
    "pdfBase64": "JVBERi0x..."
}
```

---

## Segurança

- Senhas armazenadas com hash BCrypt — nunca em texto puro
- Rotas protegidas por JWT — sem token, sem acesso
- Cada usuário acessa apenas seus próprios dados
- Credenciais nunca sobem para o repositório via `.env`

---

## Acesso em rede local

Para acessar o sistema em outros dispositivos da mesma rede, adicione no `Program.cs`:

```csharp
app.Urls.Add("http://0.0.0.0:5284");
```

Descubra o IP da máquina com `ipconfig` e acesse pelo IP local, ex: `http://192.168.1.100:5284`.

---

## Autor

Desenvolvido por **Gustavo Fiocco**

[![GitHub](https://img.shields.io/badge/GitHub-GITGUSH-181717?style=flat&logo=github)](https://github.com/GITGUSH)