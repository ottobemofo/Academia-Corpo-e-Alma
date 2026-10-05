using Microsoft.AspNetCore.Mvc;
using GestaoAcademia.Models;

namespace GestaoAcademia.Controllers;

public class PagamentoController : Controller
{
    private static List<Pagamento> pagamentos =
        new List<Pagamento>();


    public ActionResult Index()
    {
        return View(pagamentos);
    }


    [HttpGet]
    public ActionResult Create()
    {
        return View();
    }


    [HttpPost]
    public ActionResult Create(Pagamento model)
    {
        model.IdPagamento =
            pagamentos.Count == 0
            ? 1
            : pagamentos.Max(p => p.IdPagamento) + 1;

        pagamentos.Add(model);

        return RedirectToAction("Index");
    }


    [HttpGet]
    public ActionResult Update(int id)
    {
        Pagamento? pagamento =
            pagamentos.FirstOrDefault(
                p => p.IdPagamento == id);

        if (pagamento == null)
        {
            return NotFound();
        }

        return View(pagamento);
    }


    [HttpPost]
    public ActionResult Update(
        int id,
        Pagamento model)
    {
        Pagamento? pagamento =
            pagamentos.FirstOrDefault(
                p => p.IdPagamento == id);

        if (pagamento == null)
        {
            return NotFound();
        }

        pagamento.IdMatricula =
            model.IdMatricula;

        pagamento.DataPagamento =
            model.DataPagamento;

        pagamento.FormaPagamento =
            model.FormaPagamento;

        return RedirectToAction("Index");
    }


    public ActionResult Delete(int id)
    {
        Pagamento? pagamento =
            pagamentos.FirstOrDefault(
                p => p.IdPagamento == id);

        if (pagamento != null)
        {
            pagamentos.Remove(pagamento);
        }

        return RedirectToAction("Index");
    }
}