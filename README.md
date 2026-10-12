# Gerenciador de Tarefas — Backend

API para cadastro de usuários, autenticação e gerenciamento de tarefas, desenvolvida como projeto de estudo do squad Jaqueline Goes, do bootcamp WoMakersCode.

## Tecnologias

- C# e .NET 10
- ASP.NET Core
- Entity Framework Core 10
- SQLite
- Swagger / Swashbuckle
- Autenticação por cookie
- Hash de senhas com PasswordHasher

## Funcionalidades

- Cadastro de usuários.
- Login com e-mail e senha.
- Criação e listagem de tarefas por usuário.
- Consulta de tarefas por ID.
- Edição, conclusão e exclusão de tarefas.
- Tratamento centralizado de exceções.
- Documentação interativa pelo Swagger.

## Pré-requisitos

- SDK do .NET 10.
- Git.

Para conferir os SDKs instalados:

```bash
dotnet --list-sdks
```

## Como executar

### 1. Clonar o repositório

```bash
git clone https://github.com/womakers-squad-jaqueline-goes/gerenciador-de-tarefas-backend.git
cd gerenciador-de-tarefas-backend
```

Para utilizar a versão da branch de desenvolvimento:

```bash
git switch develop
git pull origin develop
```

### 2. Conferir a configuração do banco

A conexão está definida no `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=gerenciador-tarefas.db"
}
```

O SQLite utiliza um arquivo local, sem necessidade de instalar um servidor de banco de dados.

Ao iniciar, a aplicação executa o `DbInitializer`, responsável pela inicialização da base.

Os arquivos do banco estão no `.gitignore`. Portanto, os dados cadastrados localmente não são enviados ao GitHub.

### 3. Restaurar as dependências e compilar

Na pasta que contém o arquivo `.csproj`, execute:

```bash
dotnet restore
dotnet build
```

### 4. Iniciar a API

Para executar em ambiente de desenvolvimento na porta utilizada pelo frontend:

```bash
dotnet run --no-launch-profile --urls http://localhost:5161 --environment Development
```

Mantenha o terminal aberto enquanto utiliza a aplicação.

Para encerrar, pressione `Ctrl + C`.

### 5. Acessar o Swagger

Com a API em execução, abra:

http://localhost:5161

O Swagger está disponível na raiz da aplicação em ambiente `Development`.

Nele, é possível consultar os endpoints, visualizar os dados exigidos por cada operação e executar requisições.

## Frontend

A interface Angular está disponível no repositório:

[Gerenciador de Tarefas — Frontend](https://github.com/womakers-squad-jaqueline-goes/gerenciador-de-tarefas-frontend)

Siga as instruções do frontend para instalar as dependências e iniciar a interface.

### Endereços utilizados na integração local

| Aplicação | Endereço |
| --- | --- |
| Backend | http://localhost:5161 |
| Frontend | http://localhost:4200 |

O backend possui uma política CORS que permite requisições de `http://localhost:4200`.

As chamadas do Angular que utilizam o cookie de autenticação devem incluir:

```typescript
{ withCredentials: true }
```

Para testar a integração:

1. Inicie o backend.
2. Inicie o frontend em outro terminal.
3. Cadastre um usuário.
4. Faça login com o mesmo e-mail e senha.
5. Acesse as funcionalidades disponíveis na interface.

## Estrutura do projeto

| Pasta | Responsabilidade |
| --- | --- |
| `Controllers` | Endpoints HTTP |
| `DTOs` | Dados de entrada e saída |
| `Models` | Entidades da aplicação |
| `Enums` | Status das tarefas |
| `Services` | Regras de negócio e autenticação |
| `Repositories` | Acesso ao banco de dados |
| `Data` | DbContext e inicialização da base |
| `Migrations` | Evolução da estrutura do banco |
| `Middleware` | Tratamento centralizado de exceções |
| `Exceptions` | Exceções da aplicação |

## Observações para testes

- Os IDs de usuários e tarefas são GUIDs. Utilize os IDs retornados pela API.
- Os status das tarefas são retornados como texto: `Pendente` e `Concluida`.
- A edição de tarefas utiliza `PATCH`.
- A conclusão utiliza `PATCH` na rota `/api/usuarios/{usuarioId}/tarefas/{id}/concluir`.
- A senha recebe hash durante o cadastro. No login, informe a senha original.
- Cada pessoa possui sua própria base SQLite local.
- Consulte o Swagger para verificar os endpoints e os formatos das requisições.
