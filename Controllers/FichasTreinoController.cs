using System.Security.Claims;
using GestaoAcademia.Data;
using GestaoAcademia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GestaoAcademia.Controllers;

public class FichasTreinoController : Controller
{
    private readonly AppDbContext _db;
    public FichasTreinoController(AppDbContext db) => _db = db;

    private int UsuarioLogadoId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    private async Task CarregarListas(FichaTreino? f = null)
    {
        ViewBag.Alunos = new SelectList(
            await _db.Alunos.OrderBy(a => a.Nome).ToListAsync(), "IdPessoa", "Nome", f?.AlunoId);

        var exercicios = await _db.Exercicios
            .OrderBy(e => e.GrupoMuscular).ThenBy(e => e.NomeExercicio)
            .Select(e => new { e.IdExercicio, Texto = e.NomeExercicio + " (" + e.GrupoMuscular + ")" })
            .ToListAsync();
        ViewBag.Exercicios = new SelectList(exercicios, "IdExercicio", "Texto");
    }

    public async Task<IActionResult> Index()
    {
        var lista = await _db.FichasTreino
            .Include(f => f.Aluno)
            .Include(f => f.Exercicios)
            .OrderByDescending(f => f.DataCriacao)
            .ToListAsync();
        return View(lista);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var f = await _db.FichasTreino
            .Include(x => x.Aluno).Include(x => x.Usuario).Include(x => x.Exercicios)
            .FirstOrDefaultAsync(x => x.IdTreino == id);
        if (f == null) return NotFound();
        return View(f);
    }

    public async Task<IActionResult> Create()
    {
        await CarregarListas();
        return View(new FichaTreino());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FichaTreino ficha)
    {
        ficha.UsuarioId = UsuarioLogadoId;   // relacionamento "monta"
        if (!ModelState.IsValid)
        {
            await CarregarListas(ficha);
            return View(ficha);
        }

        // Converte os ids marcados no formulário nos objetos Exercicio (relação N:N)
        ficha.Exercicios = await _db.Exercicios
            .Where(e => ficha.ExerciciosIds.Contains(e.IdExercicio)).ToListAsync();

        _db.FichasTreino.Add(ficha);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = ficha.IdTreino });
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var f = await _db.FichasTreino.Include(x => x.Exercicios)
            .FirstOrDefaultAsync(x => x.IdTreino == id);
        if (f == null) return NotFound();
        f.ExerciciosIds = f.Exercicios.Select(e => e.IdExercicio).ToList();
        await CarregarListas(f);
        return View(f);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, FichaTreino form)
    {
        if (id != form.IdTreino) return NotFound();
        if (!ModelState.IsValid)
        {
            await CarregarListas(form);
            return View(form);
        }

        var ficha = await _db.FichasTreino.Include(x => x.Exercicios)
            .FirstOrDefaultAsync(x => x.IdTreino == id);
        if (ficha == null) return NotFound();

        // Copia só o que o usuário pode alterar (evita "overposting")
        ficha.NomeTreino = form.NomeTreino;
        ficha.DataCriacao = form.DataCriacao;
        ficha.Objetivo = form.Objetivo;
        ficha.Series = form.Series;
        ficha.Repeticoes = form.Repeticoes;
        ficha.CargaKg = form.CargaKg;
        ficha.AlunoId = form.AlunoId;

        ficha.Exercicios.Clear();
        var escolhidos = await _db.Exercicios
            .Where(e => form.ExerciciosIds.Contains(e.IdExercicio)).ToListAsync();
        foreach (var e in escolhidos) ficha.Exercicios.Add(e);

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id });
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var f = await _db.FichasTreino
            .Include(x => x.Aluno).Include(x => x.Usuario).Include(x => x.Exercicios)
            .FirstOrDefaultAsync(x => x.IdTreino == id);
        if (f == null) return NotFound();
        return View(f);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var f = await _db.FichasTreino.Include(x => x.Exercicios)
            .FirstOrDefaultAsync(x => x.IdTreino == id);
        if (f != null)
        {
            _db.FichasTreino.Remove(f);   // as linhas da tabela de junção saem junto
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
