using System.Security.Claims;
using GestaoAcademia.Data;
using GestaoAcademia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GestaoAcademia.Controllers;

public class MatriculasController : Controller
{
    private readonly AppDbContext _db;
    public MatriculasController(AppDbContext db) => _db = db;

    // Id do usuário logado (guardado no cookie no momento do login)
    private int UsuarioLogadoId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // Preenche os <select> do formulário
    private async Task CarregarListas(Matricula? m = null)
    {
        ViewBag.Alunos = new SelectList(
            await _db.Alunos.OrderBy(a => a.Nome).ToListAsync(), "IdPessoa", "Nome", m?.AlunoId);

        ViewBag.Planos = new SelectList(
            await _db.Planos
                .Where(p => p.StatusPlano || (m != null && p.IdPlano == m.PlanoId))
                .OrderBy(p => p.NomePlano).ToListAsync(),
            "IdPlano", "NomePlano", m?.PlanoId);
    }

    public async Task<IActionResult> Index()
    {
        var lista = await _db.Matriculas
            .Include(m => m.Aluno)
            .Include(m => m.Plano)
            .Include(m => m.Usuario)
            .Include(m => m.Pagamentos)
            .OrderByDescending(m => m.DataMatricula)
            .ToListAsync();
        return View(lista);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var m = await _db.Matriculas
            .Include(x => x.Aluno)
            .Include(x => x.Plano)
            .Include(x => x.Usuario)
            .Include(x => x.Pagamentos)
            .FirstOrDefaultAsync(x => x.IdMatricula == id);
        if (m == null) return NotFound();
        return View(m);
    }

    public async Task<IActionResult> Create()
    {
        await CarregarListas();
        return View(new Matricula { DiaPagamento = Math.Min(DateTime.Today.Day, 28) });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Matricula matricula)
    {
        matricula.UsuarioId = UsuarioLogadoId;   // relacionamento "registra"
        if (!ModelState.IsValid)
        {
            await CarregarListas(matricula);
            return View(matricula);
        }
        _db.Matriculas.Add(matricula);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = matricula.IdMatricula });
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var m = await _db.Matriculas.FindAsync(id.Value);
        if (m == null) return NotFound();
        await CarregarListas(m);
        return View(m);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Matricula matricula)
    {
        if (id != matricula.IdMatricula) return NotFound();
        if (!ModelState.IsValid)
        {
            await CarregarListas(matricula);
            return View(matricula);
        }
        _db.Update(matricula);   // UsuarioId original vem no campo hidden do formulário
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id });
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var m = await _db.Matriculas
            .Include(x => x.Aluno).Include(x => x.Plano).Include(x => x.Usuario).Include(x => x.Pagamentos)
            .FirstOrDefaultAsync(x => x.IdMatricula == id);
        if (m == null) return NotFound();
        return View(m);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var m = await _db.Matriculas.FindAsync(id);
        if (m != null)
        {
            try
            {
                _db.Matriculas.Remove(m);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["Erro"] = "Não é possível excluir: a matrícula possui pagamentos.";
            }
        }
        return RedirectToAction(nameof(Index));
    }
}
