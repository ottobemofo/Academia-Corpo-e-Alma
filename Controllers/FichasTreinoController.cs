using Microsoft.AspNetCore.Mvc;
using GestaoAcademia.Models;

namespace GestaoAcademia.Controllers;

public class FichaTreinoController : Controller
{
    public ActionResult Index()
    {
        List<FichaTreino> fichas = new List<FichaTreino>();

        return View(fichas);
    }

    [HttpGet]
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(FichaTreino model)
    {
        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Update(int id)
    {
        FichaTreino ficha = new FichaTreino();

        return View(ficha);
    }

    [HttpPost]
    public ActionResult Update(int id, FichaTreino model)
    {
        return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
        return RedirectToAction("Index");
    }
}