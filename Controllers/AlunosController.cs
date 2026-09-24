using GestaoAcademia.Data;
using GestaoAcademia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoAcademia.Controllers;

public class AlunosController : Controller
{
    private readonly AppDbContext _db;
    public AlunosController(AppDbContext db) => _db = db;

    // LISTAR
    public async Task<IActionResult> Index()
        => View(await _db.Alunos.OrderBy(x => x.Nome).ToListAsync());

    // DETALHES
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var item = await _db.Alunos.FindAsync(id.Value);
        if (item == null) return NotFound();
        return View(item);
    }

    // CRIAR (GET mostra o formulário, POST salva)
    public IActionResult Create() => View(new Aluno());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Aluno item)
    {
        if (!ModelState.IsValid) return View(item);
        _db.Alunos.Add(item);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // EDITAR
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var item = await _db.Alunos.FindAsync(id.Value);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Aluno item)
    {
        if (id != item.IdPessoa) return NotFound();
        if (!ModelState.IsValid) return View(item);
        _db.Update(item);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // EXCLUIR (GET pede confirmação, POST exclui)
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var item = await _db.Alunos.FindAsync(id.Value);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _db.Alunos.FindAsync(id);
        if (item != null)
        {
            try
            {
                _db.Alunos.Remove(item);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["Erro"] = "Não é possível excluir: existem registros vinculados a este item.";
            }
        }
        return RedirectToAction(nameof(Index));
    }
}
