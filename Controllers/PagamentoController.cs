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

        AtualizarMatricula(model.IdMatricula);

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
        Pagamento pagamentoAnterior = pagamentoRepository.Read(id);

        if (pagamentoAnterior == null)
        {
            return NotFound();
        }

        int idMatriculaAnterior = pagamentoAnterior.IdMatricula;

        model.IdPagamento = id;

        pagamentoRepository.Update(model);

        AtualizarMatricula(idMatriculaAnterior);

        if (model.IdMatricula != idMatriculaAnterior)
        {
            AtualizarMatricula(model.IdMatricula);
        }

        return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
        Pagamento pagamento = pagamentoRepository.Read(id);

        if (pagamento == null)
        {
            return NotFound();
        }

        int idMatricula = pagamento.IdMatricula;

        pagamentoRepository.Delete(id);

        AtualizarMatricula(idMatricula);

        return RedirectToAction("Index");
    }

    private void AtualizarMatricula(int idMatricula)
    {
        List<Matricula> matriculas = matriculaRepository.Read();

        Matricula? matriculaEncontrada = null;

        foreach (Matricula matricula in matriculas)
        {
            if (matricula.IdMatricula == idMatricula)
            {
                matriculaEncontrada = matricula;
                break;
            }
        }

        if (matriculaEncontrada == null)
        {
            return;
        }

        List<Pagamento> pagamentos = pagamentoRepository.Read();

        DateTime? ultimoPagamento = null;

        foreach (Pagamento pagamento in pagamentos)
        {
            if (pagamento.IdMatricula == idMatricula)
            {
                if (!ultimoPagamento.HasValue ||
                    pagamento.DataPagamento > ultimoPagamento.Value)
                {
                    ultimoPagamento = pagamento.DataPagamento;
                }
            }
        }

        matriculaEncontrada.UltimoPagamento = ultimoPagamento;

        matriculaEncontrada.MesesEmAtraso = CalcularMesesEmAtraso(
            ultimoPagamento,
            matriculaEncontrada.DiaPagamento
        );

        matriculaRepository.Update(matriculaEncontrada);
    }

    private int CalcularMesesEmAtraso(
        DateTime? ultimoPagamento,
        int diaPagamento)
    {
        DateTime hoje = DateTime.Today;

        int diaVencimento = Math.Min(
            diaPagamento,
            DateTime.DaysInMonth(hoje.Year, hoje.Month)
        );

        DateTime vencimentoAtual = new DateTime(
            hoje.Year,
            hoje.Month,
            diaVencimento
        );

        DateTime mesQueDeveriaEstarPago;

        if (hoje < vencimentoAtual)
        {
            mesQueDeveriaEstarPago = vencimentoAtual.AddMonths(-1);
        }
        else
        {
            mesQueDeveriaEstarPago = vencimentoAtual;
        }

        if (!ultimoPagamento.HasValue)
        {
            if (hoje >= vencimentoAtual)
            {
                return 1;
            }

            return 0;
        }

        DateTime pagamento = ultimoPagamento.Value;

        int mesesEmAtraso =
            (mesQueDeveriaEstarPago.Year - pagamento.Year) * 12 +
            (mesQueDeveriaEstarPago.Month - pagamento.Month);

        if (mesesEmAtraso < 0)
        {
            mesesEmAtraso = 0;
        }

        return mesesEmAtraso;
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