using System.Security.Claims;
using GestaoAcademia.Data;
using GestaoAcademia.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoAcademia.Controllers;

[AllowAnonymous]
public class ContaController : Controller
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasher<Usuario> _hasher;

    public ContaController(AppDbContext db, IPasswordHasher<Usuario> hasher)
    {
        _db = db;
        _hasher = hasher;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string email, string senha, string? returnUrl = null)
    {
        var usuario = await _db.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

        if (usuario == null ||
            _hasher.VerifyHashedPassword(usuario, usuario.Senha, senha ?? "") == PasswordVerificationResult.Failed)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Erro"] = "E-mail ou senha inválidos.";
            return View();
        }

        // "Claims" = informações guardadas no cookie de login
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.IdPessoa.ToString()),
            new(ClaimTypes.Name, usuario.Nome),
            new(ClaimTypes.Role, usuario.Perfil.ToString())
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

        return Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl!) : RedirectToAction("Index", "Home");
    }

    [HttpPost, ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    public IActionResult AcessoNegado() => View();
}
