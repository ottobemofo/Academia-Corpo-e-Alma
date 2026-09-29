using GestaoAcademia.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoAcademia.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;
    public HomeController(AppDbContext db) => _db = db;

    // Painel: matrículas com mensalidade em atraso
    public async Task<IActionResult> Index()
    {
        var matriculas = await _db.Matriculas
            .Include(m => m.Aluno)
            .Include(m => m.Plano)
            .Include(m => m.Pagamentos)   // necessário para calcular UltimoPagamento/MesesEmAtraso
            .ToListAsync();

        // MesesEmAtraso é calculado em C# (não existe no banco), por isso o filtro vem depois do ToList
        var emAtraso = matriculas
            .Where(m => m.MesesEmAtraso > 0)
            .OrderByDescending(m => m.MesesEmAtraso)
            .ToList();

        return View(emAtraso);
    }
}
