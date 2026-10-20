using Microsoft.AspNetCore.Mvc;
using GestaoAcademia.Models;
using GestaoAcademia.Repositories;

namespace GestaoAcademia.Controllers;

public class AlunoController : Controller
{
    private IAlunoRepository alunoRepository;
    private IMatriculaRepository matriculaRepository;
    private IPlanoRepository planoRepository;
    private IUsuarioRepository usuarioRepository;


    public AlunoController(
        IAlunoRepository alunoRepository,
        IMatriculaRepository matriculaRepository,
        IPlanoRepository planoRepository,
        IUsuarioRepository usuarioRepository)
    {
        this.alunoRepository = alunoRepository;
        this.matriculaRepository = matriculaRepository;
        this.planoRepository = planoRepository;
        this.usuarioRepository = usuarioRepository;
    }


    public ActionResult Index()
    {
        List<Aluno> alunos =
            alunoRepository.Read();

        List<Matricula> matriculas =
            matriculaRepository.Read();

        List<Plano> planos =
            planoRepository.Read();

        List<Usuario> usuarios =
            usuarioRepository.Read();


        List<AlunoListaViewModel> lista =
            new List<AlunoListaViewModel>();


        foreach (Aluno aluno in alunos)
        {
            Matricula? matriculaDoAluno =
                null;


            foreach (Matricula matricula in matriculas)
            {
                if (matricula.IdAluno ==
                    aluno.IdPessoa)
                {
                    matriculaDoAluno =
                        matricula;

                    break;
                }
            }


            string nomePlano = "-";

            string nomeFuncionario = "-";

            DateTime dataMatricula =
                DateTime.Today;

            int diaPagamento = 0;

            DateTime? ultimoPagamento =
                null;

            int mesesEmAtraso = 0;

            StatusAluno statusAluno =
                StatusAluno.EmDia;


            if (matriculaDoAluno != null)
            {
                dataMatricula =
                    matriculaDoAluno.DataMatricula;

                diaPagamento =
                    matriculaDoAluno.DiaPagamento;

                ultimoPagamento =
                    matriculaDoAluno.UltimoPagamento;


                mesesEmAtraso =
                    CalcularMesesEmAtraso(
                        ultimoPagamento,
                        diaPagamento
                    );


                statusAluno =
                    DeterminarStatusAluno(
                        mesesEmAtraso
                    );


                foreach (Plano plano in planos)
                {
                    if (plano.IdPlano ==
                        matriculaDoAluno.IdPlano)
                    {
                        nomePlano =
                            plano.NomePlano;

                        break;
                    }
                }


                foreach (Usuario usuario in usuarios)
                {
                    if (usuario.IdPessoa ==
                        matriculaDoAluno.IdUsuario)
                    {
                        nomeFuncionario =
                            usuario.Nome;

                        break;
                    }
                }
            }


            AlunoListaViewModel item =
                new AlunoListaViewModel();


            item.IdPessoa =
                aluno.IdPessoa;

            item.Nome =
                aluno.Nome;

            item.Telefone =
                aluno.Telefone;

            item.StatusAluno =
                statusAluno;

            item.Plano =
                nomePlano;

            item.DataMatricula =
                dataMatricula;

            item.DiaPagamento =
                diaPagamento;

            item.UltimoPagamento =
                ultimoPagamento;

            item.MesesEmAtraso =
                mesesEmAtraso;

            item.Funcionario =
                nomeFuncionario;


            lista.Add(item);
        }


        return View(lista);
    }


    [HttpGet]
    public ActionResult Create()
    {
        List<Plano> planos =
            planoRepository.Read();

        List<Usuario> usuarios =
            usuarioRepository.Read();


        List<Usuario> funcionarios =
            new List<Usuario>();


        foreach (Usuario usuario in usuarios)
        {
            if (usuario.Perfil ==
                PerfilUsuario.Funcionario)
            {
                funcionarios.Add(usuario);
            }
        }


        ViewBag.Planos =
            planos;

        ViewBag.Funcionarios =
            funcionarios;


        return View();
    }


    [HttpPost]
    public ActionResult Create(
        AlunoFormViewModel model)
    {
        Aluno aluno =
            new Aluno();


        aluno.Nome =
            model.Nome;

        aluno.Telefone =
            model.Telefone;

        aluno.StatusAluno =
            StatusAluno.EmDia;


        alunoRepository.Create(aluno);


        Matricula matricula =
            new Matricula();


        matricula.IdAluno =
            aluno.IdPessoa;

        matricula.IdPlano =
            model.IdPlano;

        matricula.IdUsuario =
            model.IdUsuario;

        matricula.DataMatricula =
            DateTime.Today;

        matricula.DiaPagamento =
            model.DiaPagamento;

        matricula.UltimoPagamento =
            null;


        matricula.MesesEmAtraso =
            CalcularMesesEmAtraso(
                null,
                model.DiaPagamento
            );


        matriculaRepository.Create(
            matricula
        );


        return RedirectToAction("Index");
    }


    [HttpGet]
    public ActionResult Update(int id)
    {
        Aluno aluno =
            alunoRepository.Read(id);


        if (aluno == null)
        {
            return NotFound();
        }


        Matricula? matriculaDoAluno =
            null;


        List<Matricula> matriculas =
            matriculaRepository.Read();


        foreach (Matricula matricula in matriculas)
        {
            if (matricula.IdAluno == id)
            {
                matriculaDoAluno =
                    matricula;

                break;
            }
        }


        AlunoFormViewModel model =
            new AlunoFormViewModel();


        model.IdPessoa =
            aluno.IdPessoa;

        model.Nome =
            aluno.Nome;

        model.Telefone =
            aluno.Telefone;


        if (matriculaDoAluno != null)
        {
            model.IdPlano =
                matriculaDoAluno.IdPlano;

            model.DiaPagamento =
                matriculaDoAluno.DiaPagamento;

            model.IdUsuario =
                matriculaDoAluno.IdUsuario;
        }


        List<Plano> planos =
            planoRepository.Read();

        List<Usuario> usuarios =
            usuarioRepository.Read();


        List<Usuario> funcionarios =
            new List<Usuario>();


        foreach (Usuario usuario in usuarios)
        {
            if (usuario.Perfil ==
                PerfilUsuario.Funcionario)
            {
                funcionarios.Add(usuario);
            }
        }


        ViewBag.Planos =
            planos;

        ViewBag.Funcionarios =
            funcionarios;


        return View(model);
    }


    [HttpPost]
    public ActionResult Update(
        int id,
        AlunoFormViewModel model)
    {
        Aluno aluno =
            alunoRepository.Read(id);


        if (aluno == null)
        {
            return NotFound();
        }


        aluno.Nome =
            model.Nome;

        aluno.Telefone =
            model.Telefone;


        alunoRepository.Update(
            aluno
        );


        Matricula? matriculaDoAluno =
            null;


        List<Matricula> matriculas =
            matriculaRepository.Read();


        foreach (Matricula matricula in matriculas)
        {
            if (matricula.IdAluno == id)
            {
                matriculaDoAluno =
                    matricula;

                break;
            }
        }


        if (matriculaDoAluno != null)
        {
            matriculaDoAluno.IdPlano =
                model.IdPlano;

            matriculaDoAluno.IdUsuario =
                model.IdUsuario;

            matriculaDoAluno.DiaPagamento =
                model.DiaPagamento;


            matriculaDoAluno.MesesEmAtraso =
                CalcularMesesEmAtraso(
                    matriculaDoAluno.UltimoPagamento,
                    model.DiaPagamento
                );


            matriculaRepository.Update(
                matriculaDoAluno
            );
        }


        return RedirectToAction("Index");
    }


    private StatusAluno DeterminarStatusAluno(
        int mesesEmAtraso)
    {
        if (mesesEmAtraso >= 3)
        {
            return StatusAluno.Inativo;
        }


        if (mesesEmAtraso > 0)
        {
            return StatusAluno.Atrasado;
        }


        return StatusAluno.EmDia;
    }


    private int CalcularMesesEmAtraso(
        DateTime? ultimoPagamento,
        int diaPagamento)
    {
        DateTime hoje =
            DateTime.Today;


        int diaVencimento =
            Math.Min(
                diaPagamento,
                DateTime.DaysInMonth(
                    hoje.Year,
                    hoje.Month
                )
            );


        DateTime vencimentoAtual =
            new DateTime(
                hoje.Year,
                hoje.Month,
                diaVencimento
            );


        DateTime mesQueDeveriaEstarPago;


        if (hoje < vencimentoAtual)
        {
            mesQueDeveriaEstarPago =
                vencimentoAtual.AddMonths(-1);
        }
        else
        {
            mesQueDeveriaEstarPago =
                vencimentoAtual;
        }


        if (!ultimoPagamento.HasValue)
        {
            if (hoje >= vencimentoAtual)
            {
                return 1;
            }

            return 0;
        }


        DateTime pagamento =
            ultimoPagamento.Value;


        int mesesEmAtraso =
            (mesQueDeveriaEstarPago.Year -
             pagamento.Year) * 12
            +
            (mesQueDeveriaEstarPago.Month -
             pagamento.Month);


        if (mesesEmAtraso < 0)
        {
            mesesEmAtraso = 0;
        }


        return mesesEmAtraso;
    }
}