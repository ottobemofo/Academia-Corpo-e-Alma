using System.ComponentModel.DataAnnotations;

namespace GestaoAcademia.Models;

public class Plano
{
    [Key]
    public int IdPlano { get; set; }

    [Required(ErrorMessage = "Informe o nome do plano.")]
    [StringLength(80)]
    [Display(Name = "Nome do plano")]
    public string NomePlano { get; set; } = "";

    [StringLength(300)]
    [Display(Name = "Descrição")]
    public string? Descricao { get; set; }

    [Display(Name = "Ativo")]
    public bool StatusPlano { get; set; } = true;

    public ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
}
