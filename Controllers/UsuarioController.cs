using Microsoft.AspNetCore.Mvc;
using GestaoAcademia.Models;
using GestaoAcademia.Repositories;

namespace GestaoAcademia.Controllers;

public class UsuarioController : Controller
{
    private IUsuarioRepository repository;

    public UsuarioController(IUsuarioRepository repository)
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
    public ActionResult Create(Usuario model)
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
    public ActionResult Update(int id, Usuario model)
    {
        model.IdPessoa = id;
        repository.Update(model);
        return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
        repository.Delete(id);
        return RedirectToAction("Index");
    }
}
