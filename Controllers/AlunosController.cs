using Microsoft.AspNetCore.Mvc;
using GestaoAcademia.Models;

namespace GestaoAcademia.Controllers;

public class AlunoController : Controller
{
    public ActionResult Index()
    {
        List<AlunoListaViewModel> alunos =
            new List<AlunoListaViewModel>();

        return View(alunos);
    }


    [HttpGet]
    public ActionResult Create()
    {
        return View();
    }


    [HttpPost]
    public ActionResult Create(AlunoFormViewModel aluno)
    {
        return RedirectToAction("Index");
    }


    [HttpGet]
    public ActionResult Update(int id)
    {
        AlunoFormViewModel aluno =
            new AlunoFormViewModel();

        aluno.IdPessoa = id;

        return View(aluno);
    }


    [HttpPost]
    public ActionResult Update(
        int id,
        AlunoFormViewModel aluno)
    {
        return RedirectToAction("Index");
    }
}