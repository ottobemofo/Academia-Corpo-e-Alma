using Microsoft.AspNetCore.Mvc;
using GestaoAcademia.Models;
using GestaoAcademia.Repositories;

namespace GestaoAcademia.Controllers;

public class PagamentoController : Controller
{
    private IPagamentoRepository pagamentoRepository;
    private IMatriculaRepository matriculaRepository;
    private IAlunoRepository alunoRepository;

    public PagamentoController(
        IPagamentoRepository pagamentoRepository,
        IMatriculaRepository matriculaRepository,
        IAlunoRepository alunoRepository)
    {
        this.pagamentoRepository = pagamentoRepository;
        this.matriculaRepository = matriculaRepository;
        this.alunoRepository = alunoRepository;
    }

    public ActionResult Index()
    {
        ViewBag.AlunosMatriculas = MontarListaAlunos();

        return View(pagamentoRepository.Read());
    }

    [HttpGet]
    public ActionResult Create()
    {
        ViewBag.AlunosMatriculas = MontarListaAlunos();

        return View();
    }

    [HttpPost]
    public ActionResult Create(Pagamento model)
    {
        pagamentoRepository.Create(model);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Update(int id)
    {
        Pagamento pagamento = pagamentoRepository.Read(id);

        if (pagamento == null)
        {
            return NotFound();
        }

        ViewBag.AlunosMatriculas = MontarListaAlunos();

        return View(pagamento);
    }

    [HttpPost]
    public ActionResult Update(int id, Pagamento model)
    {
        model.IdPagamento = id;

        pagamentoRepository.Update(model);

        return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
        pagamentoRepository.Delete(id);

        return RedirectToAction("Index");
    }

    private Dictionary<int, string> MontarListaAlunos()
    {
        List<Matricula> matriculas = matriculaRepository.Read();
        List<Aluno> alunos = alunoRepository.Read();

        Dictionary<int, string> alunosMatriculas =
            new Dictionary<int, string>();

        foreach (Matricula matricula in matriculas)
        {
            foreach (Aluno aluno in alunos)
            {
                if (aluno.IdPessoa == matricula.IdAluno)
                {
                    alunosMatriculas.Add(
                        matricula.IdMatricula,
                        aluno.Nome
                    );

                    break;
                }
            }
        }

        return alunosMatriculas;
    }
}