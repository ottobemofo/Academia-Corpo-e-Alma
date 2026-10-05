using Microsoft.AspNetCore.Mvc;

namespace GestaoAcademia.Controllers;

public class ContaController : Controller
{
    [HttpGet]
    public ActionResult Login()
    {
        return View();
    }
}