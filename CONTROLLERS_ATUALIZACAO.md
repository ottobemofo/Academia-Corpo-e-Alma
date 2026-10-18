# Controllers atualizados

Os Controllers foram ajustados para seguir o padrão do projeto do professor.

Antes, cada action criava o Repository manualmente:

```csharp
IPlanoRepository repo = new PlanoSqlRepository();
```

Agora o Repository é recebido pelo construtor:

```csharp
private IPlanoRepository repository;

public PlanoController(IPlanoRepository repository)
{
    this.repository = repository;
}
```

E as actions ficam simples:

```csharp
public ActionResult Index()
{
    return View(repository.Read());
}
```

Os arquivos de Controller também foram padronizados no singular:

- AlunoController.cs
- PlanoController.cs
- PagamentoController.cs
- ExercicioController.cs
- FichaTreinoController.cs
- MatriculaController.cs
- UsuarioController.cs

A etapa atual é CRUD com SQL Server. Autenticação/autorização será tratada na etapa seguinte.
