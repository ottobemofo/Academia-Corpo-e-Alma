using Microsoft.AspNetCore.Mvc;
using GestaoAcademia.Models;
using GestaoAcademia.Repositories;

namespace GestaoAcademia.Controllers;

public class MatriculaController : Controller
{
    private IMatriculaRepository repository;

    public MatriculaController(IMatriculaRepository repository)
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
    public ActionResult Create(Matricula model)
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
    public ActionResult Update(int id, Matricula model)
    {
        model.IdMatricula = id;
        repository.Update(model);
        return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
        repository.Delete(id);
        return RedirectToAction("Index");
    }
}
