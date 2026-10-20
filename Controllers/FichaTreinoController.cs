using Microsoft.AspNetCore.Mvc;
using GestaoAcademia.Models;
using GestaoAcademia.Repositories;

namespace GestaoAcademia.Controllers;

public class FichaTreinoController : Controller
{
    private IFichaTreinoRepository fichaTreinoRepository;
    private IAlunoRepository alunoRepository;
    private IUsuarioRepository usuarioRepository;
    private IExercicioRepository exercicioRepository;
    private IFichaExercicioRepository fichaExercicioRepository;

    public FichaTreinoController(
        IFichaTreinoRepository fichaTreinoRepository,
        IAlunoRepository alunoRepository,
        IUsuarioRepository usuarioRepository,
        IExercicioRepository exercicioRepository,
        IFichaExercicioRepository fichaExercicioRepository)
    {
        this.fichaTreinoRepository = fichaTreinoRepository;
        this.alunoRepository = alunoRepository;
        this.usuarioRepository = usuarioRepository;
        this.exercicioRepository = exercicioRepository;
        this.fichaExercicioRepository = fichaExercicioRepository;
    }

    public ActionResult Index()
    {
        return View(fichaTreinoRepository.Read());
    }

    [HttpGet]
    public ActionResult Create()
    {
        CarregarListas();

        return View();
    }

    [HttpPost]
    public ActionResult Create(
        FichaTreino model,
        List<int>? IdExercicios)
    {
        int idTreino = fichaTreinoRepository.Create(model);

        if (IdExercicios != null)
        {
            foreach (int idExercicio in IdExercicios)
            {
                FichaExercicio fichaExercicio = new FichaExercicio();

                fichaExercicio.IdTreino = idTreino;
                fichaExercicio.IdExercicio = idExercicio;

                fichaExercicioRepository.Create(fichaExercicio);
            }
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Update(int id)
    {
        FichaTreino ficha = fichaTreinoRepository.Read(id);

        if (ficha == null)
        {
            return NotFound();
        }

        CarregarListas();

        List<FichaExercicio> relacoes =
            fichaExercicioRepository.ReadByTreino(id);

        List<int> exerciciosSelecionados = new List<int>();

        foreach (FichaExercicio relacao in relacoes)
        {
            exerciciosSelecionados.Add(relacao.IdExercicio);
        }

        ViewBag.ExerciciosSelecionados = exerciciosSelecionados;

        return View(ficha);
    }

    [HttpPost]
    public ActionResult Update(
        int id,
        FichaTreino model,
        List<int>? IdExercicios)
    {
        model.IdTreino = id;

        fichaTreinoRepository.Update(model);

        if (IdExercicios == null)
        {
            IdExercicios = new List<int>();
        }

        List<FichaExercicio> atuais =
            fichaExercicioRepository.ReadByTreino(id);

        foreach (FichaExercicio atual in atuais)
        {
            bool continuaSelecionado = false;

            foreach (int idExercicio in IdExercicios)
            {
                if (idExercicio == atual.IdExercicio)
                {
                    continuaSelecionado = true;
                    break;
                }
            }

            if (!continuaSelecionado)
            {
                fichaExercicioRepository.Delete(
                    id,
                    atual.IdExercicio
                );
            }
        }

        foreach (int idExercicio in IdExercicios)
        {
            bool jaExiste = false;

            foreach (FichaExercicio atual in atuais)
            {
                if (atual.IdExercicio == idExercicio)
                {
                    jaExiste = true;
                    break;
                }
            }

            if (!jaExiste)
            {
                FichaExercicio novaRelacao = new FichaExercicio();

                novaRelacao.IdTreino = id;
                novaRelacao.IdExercicio = idExercicio;

                fichaExercicioRepository.Create(novaRelacao);
            }
        }

        return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
        fichaTreinoRepository.Delete(id);

        return RedirectToAction("Index");
    }

    private void CarregarListas()
    {
        ViewBag.Alunos = alunoRepository.Read();
        ViewBag.Usuarios = usuarioRepository.Read();
        ViewBag.Exercicios = exercicioRepository.Read();
    }
}