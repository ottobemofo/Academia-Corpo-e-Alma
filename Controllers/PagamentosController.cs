using GestaoAcademia.Data;
using GestaoAcademia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GestaoAcademia.Controllers;

public class PagamentosController : Controller
{
    private readonly AppDbContext _db;
    public PagamentosController(AppDbContext db) => _db = db;

    private async Task CarregarMatriculas(int? selecionada = null)
    {
        // Projeção: monta o texto "Aluno — Plano" para o dropdown
        var lista = await _db.Matriculas
            .OrderBy(m => m.Aluno!.Nome)
            .Select(m => new { m.IdMatricula, Texto = m.Aluno!.Nome + " — " + m.Plano!.NomePlano })
            .ToListAsync();
        ViewBag.Matriculas = new SelectList(lista, "IdMatricula", "Texto", selecionada);
    }

    public async Task<IActionResult> Index()
    {
        var lista = await _db.Pagamentos
            .Include(p => p.Matricula).ThenInclude(m => m!.Aluno)
            .Include(p => p.Matricula).ThenInclude(m => m!.Plano)
            .OrderByDescending(p => p.DataPagamento)
            .ToListAsync();
        return View(lista);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var p = await _db.Pagamentos
            .Include(x => x.Matricula).ThenInclude(m => m!.Aluno)
            .Include(x => x.Matricula).ThenInclude(m => m!.Plano)
            .FirstOrDefaultAsync(x => x.IdPagamento == id);
        if (p == null) return NotFound();
        return View(p);
    }

    // matriculaId opcional: vem do botão "Registrar pagamento" na tela da matrícula
    public async Task<IActionResult> Create(int? matriculaId)
    {
        await CarregarMatriculas(matriculaId);
        return View(new Pagamento { MatriculaId = matriculaId ?? 0 });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Pagamento pagamento)
    {
        if (!ModelState.IsValid)
        {
            await CarregarMatriculas(pagamento.MatriculaId);
            return View(pagamento);
        }
        _db.Pagamentos.Add(pagamento);
        await _db.SaveChangesAsync();
        return RedirectToAction("Details", "Matriculas", new { id = pagamento.MatriculaId });
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var p = await _db.Pagamentos.FindAsync(id.Value);
        if (p == null) return NotFound();
        await CarregarMatriculas(p.MatriculaId);
        return View(p);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Pagamento pagamento)
    {
        if (id != pagamento.IdPagamento) return NotFound();
        if (!ModelState.IsValid)
        {
            await CarregarMatriculas(pagamento.MatriculaId);
            return View(pagamento);
        }
        _db.Update(pagamento);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var p = await _db.Pagamentos
            .Include(x => x.Matricula).ThenInclude(m => m!.Aluno)
            .Include(x => x.Matricula).ThenInclude(m => m!.Plano)
            .FirstOrDefaultAsync(x => x.IdPagamento == id);
        if (p == null) return NotFound();
        return View(p);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var p = await _db.Pagamentos.FindAsync(id);
        if (p != null)
        {
            _db.Pagamentos.Remove(p);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
