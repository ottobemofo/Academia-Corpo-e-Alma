using Microsoft.AspNetCore.Mvc;
using GestaoAcademia.Models;

namespace GestaoAcademia.Controllers;

public class MatriculaController : Controller
{
    public ActionResult Index()
    {
        List<Matricula> matriculas = new List<Matricula>();

        return View(matriculas);
    }

    [HttpGet]
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(Matricula model)
    {
        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Update(int id)
    {
        Matricula matricula = new Matricula();

        return View(matricula);
    }

    [HttpPost]
    public ActionResult Update(int id, Matricula model)
    {
        return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
        return RedirectToAction("Index");
    }
}