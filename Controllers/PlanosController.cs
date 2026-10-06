using Microsoft.AspNetCore.Mvc;
using GestaoAcademia.Models;

namespace GestaoAcademia.Controllers;

public class PlanoController : Controller
{
    public ActionResult Index()
    {
        List<Plano> planos =
            new List<Plano>();

        return View(planos);
    }


    [HttpGet]
    public ActionResult Create()
    {
        return View();
    }


    [HttpPost]
    public ActionResult Create(Plano plano)
    {
        return RedirectToAction("Index");
    }


    [HttpGet]
    public ActionResult Update(int id)
    {
        Plano plano =
            new Plano();

        plano.IdPlano = id;

        return View(plano);
    }


    [HttpPost]
    public ActionResult Update(
        int id,
        Plano plano)
    {
        return RedirectToAction("Index");
    }
}