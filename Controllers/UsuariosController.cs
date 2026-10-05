using Microsoft.AspNetCore.Mvc;
using GestaoAcademia.Models;

namespace GestaoAcademia.Controllers;

public class UsuarioController : Controller
{
    public ActionResult Index()
    {
        List<Usuario> usuarios = new List<Usuario>();

        return View(usuarios);
    }

    [HttpGet]
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(Usuario model)
    {
        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Update(int id)
    {
        Usuario usuario = new Usuario();

        return View(usuario);
    }

    [HttpPost]
    public ActionResult Update(int id, Usuario model)
    {
        return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
        return RedirectToAction("Index");
    }
}