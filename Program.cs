using System.Globalization;
using GestaoAcademia.Data;
using GestaoAcademia.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Banco (SQLite). Para SQL Server: UseSqlServer + pacote Microsoft.EntityFrameworkCore.SqlServer
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlite(builder.Configuration.GetConnectionString("Padrao")));

builder.Services.AddSingleton<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();

builder.Services.AddControllersWithViews(o =>
{
    // Propriedades "string" não anuláveis não viram [Required] automaticamente
    o.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    // Tudo exige login, exceto o que tiver [AllowAnonymous]
    o.Filters.Add(new AuthorizeFilter());
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/Conta/Login";
        o.AccessDeniedPath = "/Conta/AcessoNegado";
    });

var app = builder.Build();

// Cultura pt-BR (vírgula decimal e datas dd/MM/yyyy)
var cultura = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentCulture = cultura;
CultureInfo.DefaultThreadCurrentUICulture = cultura;
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(cultura),
    SupportedCultures = new[] { cultura },
    SupportedUICultures = new[] { cultura }
});

// Cria o banco e o usuário administrador inicial
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<Usuario>>();
    db.Database.EnsureCreated();

    if (!db.Usuarios.Any())
    {
        var admin = new Usuario
        {
            Nome = "Administrador",
            Email = "admin@academia.com",
            Perfil = PerfilUsuario.Administrador
        };
        admin.Senha = hasher.HashPassword(admin, "Admin@123");
        db.Usuarios.Add(admin);
        db.SaveChanges();
    }
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.Run();
