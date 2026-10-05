using Microsoft.AspNetCore.Mvc;
using GestaoAcademia.Models;

namespace GestaoAcademia.Controllers;

public class ExercicioController : Controller
{
    public ActionResult Index()
    {
        List<Exercicio> exercicios = new List<Exercicio>();

        return View(exercicios);
    }

    [HttpGet]
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(Exercicio model)
    {
        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Update(int id)
    {
        Exercicio exercicio = new Exercicio();

        return View(exercicio);
    }

    [HttpPost]
    public ActionResult Update(int id, Exercicio model)
    {
        return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
        return RedirectToAction("Index");
    }
}