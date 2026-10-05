 using Microsoft.AspNetCore.Mvc;
using GestaoAcademia.Models;

namespace GestaoAcademia.Controllers;

public class AlunoController : Controller
{
    private static List<Aluno> alunos = new List<Aluno>();

    private static List<Matricula> matriculas = new List<Matricula>();

    public ActionResult Index()
    {
        AtualizarSituacaoAlunos();

        List<AlunoListaViewModel> lista =
            new List<AlunoListaViewModel>();

        foreach (Aluno aluno in alunos)
        {
            Matricula? matricula =
                matriculas.FirstOrDefault(
                    m => m.IdAluno == aluno.IdPessoa);

            string nomePlano = "-";
            string nomeFuncionario = "-";

            if (matricula != null)
            {
                if (matricula.IdPlano == 1)
                {
                    nomePlano = "3 vezes por semana";
                }
                else if (matricula.IdPlano == 2)
                {
                    nomePlano = "Todos os dias";
                }

                nomeFuncionario = "Funcionário";
            }

            AlunoListaViewModel item =
                new AlunoListaViewModel
                {
                    IdPessoa = aluno.IdPessoa,

                    Nome = aluno.Nome,

                    Telefone = aluno.Telefone,

                    StatusAluno = aluno.StatusAluno,

                    Plano = nomePlano,

                    DataMatricula =
                        matricula?.DataMatricula
                        ?? DateTime.Today,

                    DiaPagamento =
                        matricula?.DiaPagamento
                        ?? 0,

                    UltimoPagamento =
                        matricula?.UltimoPagamento,

                    MesesEmAtraso =
                        matricula?.MesesEmAtraso
                        ?? 0,

                    Funcionario =
                        nomeFuncionario
                };

            lista.Add(item);
        }

        return View(lista);
    }

    [HttpGet]
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(
        AlunoFormViewModel model)
    {
        int novoIdAluno =
            alunos.Count == 0
            ? 1
            : alunos.Max(a => a.IdPessoa) + 1;

        Aluno aluno =
            new Aluno
            {
                IdPessoa = novoIdAluno,

                Nome = model.Nome,

                Telefone = model.Telefone,

                StatusAluno =
                    StatusAluno.Ativo
            };

        alunos.Add(aluno);

        int novoIdMatricula =
            matriculas.Count == 0
            ? 1
            : matriculas.Max(
                m => m.IdMatricula) + 1;

        Matricula matricula =
            new Matricula
            {
                IdMatricula =
                    novoIdMatricula,

                IdAluno =
                    aluno.IdPessoa,

                IdPlano =
                    model.IdPlano,

                IdUsuario =
                    1,

                DataMatricula =
                    DateTime.Today,

                DiaPagamento =
                    model.DiaPagamento,

                UltimoPagamento =
                    model.UltimoPagamento,

                MesesEmAtraso =
                    CalcularMesesEmAtraso(
                        model.UltimoPagamento)
            };

        matriculas.Add(matricula);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Update(int id)
    {
        Aluno? aluno =
            alunos.FirstOrDefault(
                a => a.IdPessoa == id);

        if (aluno == null)
        {
            return NotFound();
        }

        Matricula? matricula =
            matriculas.FirstOrDefault(
                m => m.IdAluno == id);

        AlunoFormViewModel model =
            new AlunoFormViewModel
            {
                IdPessoa =
                    aluno.IdPessoa,

                Nome =
                    aluno.Nome,

                Telefone =
                    aluno.Telefone,

                IdPlano =
                    matricula?.IdPlano ?? 0,

                DiaPagamento =
                    matricula?.DiaPagamento ?? 0,

                UltimoPagamento =
                    matricula?.UltimoPagamento
            };

        return View(model);
    }

    [HttpPost]
    public ActionResult Update(
        int id,
        AlunoFormViewModel model)
    {
        Aluno? aluno =
            alunos.FirstOrDefault(
                a => a.IdPessoa == id);

        if (aluno == null)
        {
            return NotFound();
        }

        aluno.Nome =
            model.Nome;

        aluno.Telefone =
            model.Telefone;

        Matricula? matricula =
            matriculas.FirstOrDefault(
                m => m.IdAluno == id);

        if (matricula != null)
        {
            matricula.IdPlano =
                model.IdPlano;

            matricula.DiaPagamento =
                model.DiaPagamento;

            matricula.UltimoPagamento =
                model.UltimoPagamento;

            matricula.MesesEmAtraso =
                CalcularMesesEmAtraso(
                    model.UltimoPagamento);
        }

        AtualizarSituacaoAlunos();

        return RedirectToAction("Index");
    }

    private int CalcularMesesEmAtraso(
        DateTime? ultimoPagamento)
    {
        if (!ultimoPagamento.HasValue)
        {
            return 0;
        }

        DateTime dataPagamento =
            ultimoPagamento.Value;

        int meses =
            (DateTime.Today.Year
                - dataPagamento.Year) * 12
            + DateTime.Today.Month
                - dataPagamento.Month;

        if (meses < 0)
        {
            meses = 0;
        }

        return meses;
    }

    private void AtualizarSituacaoAlunos()
    {
        foreach (Aluno aluno in alunos)
        {
            Matricula? matricula =
                matriculas.FirstOrDefault(
                    m => m.IdAluno
                        == aluno.IdPessoa);

            if (matricula == null)
            {
                continue;
            }

            matricula.MesesEmAtraso =
                CalcularMesesEmAtraso(
                    matricula.UltimoPagamento);

            if (matricula.MesesEmAtraso >= 3)
            {
                aluno.StatusAluno =
                    StatusAluno.Inativo;
            }
            else
            {
                aluno.StatusAluno =
                    StatusAluno.Ativo;
            }
        }
    }
}