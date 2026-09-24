using GestaoAcademia.Data;
using GestaoAcademia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoAcademia.Controllers;

[Authorize(Roles = "Administrador")]
public class UsuariosController : Controller
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasher<Usuario> _hasher;

    public UsuariosController(AppDbContext db, IPasswordHasher<Usuario> hasher)
    {
        _db = db;
        _hasher = hasher;
    }

    public async Task<IActionResult> Index()
        => View(await _db.Usuarios.OrderBy(u => u.Nome).ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var item = await _db.Usuarios.FindAsync(id.Value);
        if (item == null) return NotFound();
        return View(item);
    }

    public IActionResult Create() => View(new Usuario());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Usuario usuario)
    {
        if (string.IsNullOrWhiteSpace(usuario.SenhaNova))
            ModelState.AddModelError(nameof(Usuario.SenhaNova), "Informe a senha.");
        if (await _db.Usuarios.AnyAsync(u => u.Email == usuario.Email))
            ModelState.AddModelError(nameof(Usuario.Email), "Já existe um usuário com este e-mail.");
        if (!ModelState.IsValid) return View(usuario);

        usuario.Senha = _hasher.HashPassword(usuario, usuario.SenhaNova!);
        _db.Usuarios.Add(usuario);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var item = await _db.Usuarios.FindAsync(id.Value);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Usuario form)
    {
        if (id != form.IdPessoa) return NotFound();
        if (await _db.Usuarios.AnyAsync(u => u.Email == form.Email && u.IdPessoa != id))
            ModelState.AddModelError(nameof(Usuario.Email), "Já existe um usuário com este e-mail.");
        if (!ModelState.IsValid) return View(form);

        var usuario = await _db.Usuarios.FindAsync(id);
        if (usuario == null) return NotFound();

        usuario.Nome = form.Nome;
        usuario.Email = form.Email;
        usuario.Perfil = form.Perfil;
        // Só troca a senha se algo foi digitado
        if (!string.IsNullOrWhiteSpace(form.SenhaNova))
            usuario.Senha = _hasher.HashPassword(usuario, form.SenhaNova);

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var item = await _db.Usuarios.FindAsync(id.Value);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _db.Usuarios.FindAsync(id);
        if (item != null)
        {
            try
            {
                _db.Usuarios.Remove(item);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["Erro"] = "Não é possível excluir: este usuário registrou matrículas ou montou fichas.";
            }
        }
        return RedirectToAction(nameof(Index));
    }
}
