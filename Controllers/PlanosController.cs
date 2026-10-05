using Microsoft.AspNetCore.Mvc;
using GestaoAcademia.Models;

namespace GestaoAcademia.Controllers;

public class PlanoController : Controller
{
    private static List<Plano> planos = new List<Plano>
    {
        new Plano
        {
            IdPlano = 1,
            NomePlano = "3 vezes por semana",
            ValorPlano = 70
        },

        new Plano
        {
            IdPlano = 2,
            NomePlano = "Todos os dias",
            ValorPlano = 75
        }
    };


    public ActionResult Index()
    {
        return View(planos);
    }


    [HttpGet]
    public ActionResult Create()
    {
        return View();
    }


    [HttpPost]
    public ActionResult Create(Plano model)
    {
        model.IdPlano =
            planos.Count == 0
            ? 1
            : planos.Max(p => p.IdPlano) + 1;

        planos.Add(model);

        return RedirectToAction("Index");
    }


    [HttpGet]
    public ActionResult Update(int id)
    {
        Plano? plano =
            planos.FirstOrDefault(p => p.IdPlano == id);

        if (plano == null)
        {
            return NotFound();
        }

        return View(plano);
    }


    [HttpPost]
    public ActionResult Update(
        int id,
        Plano model)
    {
        Plano? plano =
            planos.FirstOrDefault(p => p.IdPlano == id);

        if (plano == null)
        {
            return NotFound();
        }

        plano.NomePlano = model.NomePlano;

        plano.ValorPlano = model.ValorPlano;

        return RedirectToAction("Index");
    }


    public ActionResult Delete(int id)
    {
        Plano? plano =
            planos.FirstOrDefault(p => p.IdPlano == id);

        if (plano != null)
        {
            planos.Remove(plano);
        }

        return RedirectToAction("Index");
    }
}