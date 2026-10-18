# Instalação - Academia Corpo e Alma

## 1. Banco de dados

Abra o SQL Server Management Studio e execute o arquivo:

`script.sql`

O script cria o banco `ACADEMIACORPOALMA`, as tabelas e alguns registros de teste.

## 2. Conexão

O projeto segue o padrão usado em aula pelo professor com ADO.NET e `DataConnection`.

Arquivo:

`Repositories/DataConnection.cs`

Configuração padrão:

```text
Server=localhost
Database=ACADEMIACORPOALMA
Trusted_Connection=True
TrustServerCertificate=True
```

Se o seu SQL Server usa uma instância diferente, altere somente o `Server`.

Exemplos:

```text
Server=.\SQLEXPRESS
Server=DESKTOP-XXXX\SQLEXPRESS
```

## 3. Executar o projeto

No terminal, na pasta do projeto:

```powershell
dotnet restore
dotnet build
dotnet run
```

O endereço configurado no projeto é:

`http://127.0.0.1:5001`

## 4. Estrutura adotada

O projeto segue o padrão da aula:

- Models
- Controllers
- Views
- Repositories
- Interfaces de Repository
- Repositórios SQL com ADO.NET
- `DataConnection`
- Injeção dos repositórios no `Program.cs`

A autenticação e autorização ficam para a etapa de 03/11.
