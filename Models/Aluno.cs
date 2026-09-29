using System.ComponentModel.DataAnnotations;

namespace GestaoAcademia.Models;

public class Aluno : Pessoa
{
    [StringLength(20)]
    [Display(Name = "Telefone")]
    public string? Telefone { get; set; }

    [Display(Name = "Status")]
    public StatusAluno StatusAluno { get; set; } = StatusAluno.Ativo;

    // Aluno 1 --- N Matricula (faz) | Aluno 1 --- N FichaTreino (possui)
    public ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
    public ICollection<FichaTreino> Fichas { get; set; } = new List<FichaTreino>();
}
