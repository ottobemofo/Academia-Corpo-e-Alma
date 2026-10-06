using Microsoft.AspNetCore.Mvc;
using GestaoAcademia.Models;

namespace GestaoAcademia.Controllers;

public class PagamentoController : Controller
{
    public ActionResult Index()
    {
        List<Pagamento> pagamentos =
            new List<Pagamento>();

        return View(pagamentos);
    }


    [HttpGet]
    public ActionResult Create()
    {
        return View();
    }


    [HttpPost]
    public ActionResult Create(Pagamento pagamento)
    {
        return RedirectToAction("Index");
    }


    [HttpGet]
    public ActionResult Update(int id)
    {
        Pagamento pagamento =
            new Pagamento();

        pagamento.IdPagamento = id;

        return View(pagamento);
    }


    [HttpPost]
    public ActionResult Update(
        int id,
        Pagamento pagamento)
    {
        return RedirectToAction("Index");
    }
}