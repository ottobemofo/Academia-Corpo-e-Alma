using Microsoft.AspNetCore.Mvc;

namespace GestaoAcademia.Controllers;

public class HomeController : Controller
{
    public ActionResult Index()
    {
        return View();
    }
}  