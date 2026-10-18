using Microsoft.AspNetCore.Mvc;
using GestaoAcademia.Models;
using GestaoAcademia.Repositories;

namespace GestaoAcademia.Controllers;

public class PlanoController : Controller
{
    private IPlanoRepository repository;

    public PlanoController(IPlanoRepository repository)
    {
        this.repository = repository;
    }

    public ActionResult Index()
    {
        return View(repository.Read());
    }

    [HttpGet]
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(Plano model)
    {
        repository.Create(model);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Update(int id)
    {
        return View(repository.Read(id));
    }

    [HttpPost]
    public ActionResult Update(int id, Plano model)
    {
        model.IdPlano = id;
        repository.Update(model);
        return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
        repository.Delete(id);
        return RedirectToAction("Index");
    }
}
