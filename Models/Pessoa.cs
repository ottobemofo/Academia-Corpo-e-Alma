using System.ComponentModel.DataAnnotations;

namespace GestaoAcademia.Models;

// Superclasse da especialização (triângulo do DER).
// abstract: nunca existe uma "Pessoa" pura, só Aluno ou Usuario.
public abstract class Pessoa
{
    [Key]
    public int IdPessoa { get; set; }

    [Required(ErrorMessage = "Informe o nome.")]
    [StringLength(120)]
    [Display(Name = "Nome")]
    public string Nome { get; set; } = "";
}
