# ADO.NET - padrão adotado

A versão atual foi alinhada ao projeto de referência do professor.

Fluxo principal:

```text
View
  -> Controller
  -> Interface do Repository
  -> Repository SQL
  -> DataConnection
  -> SQL Server
```

Exemplo:

```text
PlanoController
  -> IPlanoRepository
  -> PlanoSqlRepository
  -> SqlCommand
  -> SQL Server
```

Os Controllers recebem os Repositories pelo construtor, conforme o padrão utilizado em aula.

O `Program.cs` registra as implementações:

```csharp
builder.Services.AddSingleton<IPlanoRepository>(new PlanoSqlRepository());
```

Os Repositories implementam:

- Create
- Read
- Read por ID
- Update
- Delete

O banco utilizado é `ACADEMIACORPOALMA` e o script está em `script.sql`.
