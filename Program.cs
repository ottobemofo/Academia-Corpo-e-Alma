using GestaoAcademia.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IAlunoRepository>(new AlunoSqlRepository());
builder.Services.AddSingleton<IUsuarioRepository>(new UsuarioSqlRepository());
builder.Services.AddSingleton<IPlanoRepository>(new PlanoSqlRepository());
builder.Services.AddSingleton<IMatriculaRepository>(new MatriculaSqlRepository());
builder.Services.AddSingleton<IPagamentoRepository>(new PagamentoSqlRepository());
builder.Services.AddSingleton<IFichaTreinoRepository>(new FichaTreinoSqlRepository());
builder.Services.AddSingleton<IExercicioRepository>(new ExercicioSqlRepository());
builder.Services.AddSingleton<IFichaExercicioRepository>(new FichaExercicioSqlRepository());

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
