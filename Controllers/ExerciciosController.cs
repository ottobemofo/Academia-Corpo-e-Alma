using GestaoAcademia.Data;
using GestaoAcademia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoAcademia.Controllers;

public class ExerciciosController : Controller
{
    private readonly AppDbContext _db;
    public ExerciciosController(AppDbContext db) => _db = db;

    // LISTAR
    public async Task<IActionResult> Index()
        => View(await _db.Exercicios.OrderBy(x => x.NomeExercicio).ToListAsync());

    // DETALHES
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var item = await _db.Exercicios.FindAsync(id.Value);
        if (item == null) return NotFound();
        return View(item);
    }

    // CRIAR (GET mostra o formulário, POST salva)
    public IActionResult Create() => View(new Exercicio());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Exercicio item)
    {
        if (!ModelState.IsValid) return View(item);
        _db.Exercicios.Add(item);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // EDITAR
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var item = await _db.Exercicios.FindAsync(id.Value);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Exercicio item)
    {
        if (id != item.IdExercicio) return NotFound();
        if (!ModelState.IsValid) return View(item);
        _db.Update(item);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // EXCLUIR (GET pede confirmação, POST exclui)
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var item = await _db.Exercicios.FindAsync(id.Value);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _db.Exercicios.FindAsync(id);
        if (item != null)
        {
            try
            {
                _db.Exercicios.Remove(item);
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
