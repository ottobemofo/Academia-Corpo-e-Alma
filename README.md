# Academia Corpo e Alma

Projeto ASP.NET Core MVC da disciplina, usando SQL Server e ADO.NET.

## Etapa atual

Entrega de 20/10:
- Controllers e Views
- Navegação
- CRUDs usando SQL Server
- Repository Pattern no padrão da aula

## Estrutura

- `Models/`
- `Controllers/`
- `Views/`
- `Repositories/`
- `Program.cs`
- `script.sql`

## Banco

Execute `script.sql` no SQL Server antes de iniciar o projeto.

Depois confira a conexão em:

`Repositories/DataConnection.cs`

## Execução

```powershell
dotnet restore
dotnet build
dotnet run
```

A aplicação está configurada para:

`http://127.0.0.1:5001`
