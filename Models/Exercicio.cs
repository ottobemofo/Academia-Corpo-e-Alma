using System.ComponentModel.DataAnnotations;

namespace GestaoAcademia.Models;

public class Exercicio
{
    [Key]
    public int IdExercicio { get; set; }

    [Required(ErrorMessage = "Informe o nome do exercício.")]
    [StringLength(100)]
    [Display(Name = "Exercício")]
    public string NomeExercicio { get; set; } = "";

    [Required(ErrorMessage = "Informe o grupo muscular.")]
    [StringLength(60)]
    [Display(Name = "Grupo muscular")]
    public string GrupoMuscular { get; set; } = "";

    // N:N com FichaTreino (relacionamento "contem")
    public ICollection<FichaTreino> Fichas { get; set; } = new List<FichaTreino>();
}
