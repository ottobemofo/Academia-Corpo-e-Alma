using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoAcademia.Models;

public class Usuario : Pessoa
{
    [Required(ErrorMessage = "Informe o e-mail.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    [StringLength(150)]
    [Display(Name = "E-mail")]
    public string Email { get; set; } = "";

    // Guarda o HASH da senha, nunca a senha em texto puro.
    public string Senha { get; set; } = "";

    [Display(Name = "Perfil")]
    public PerfilUsuario Perfil { get; set; } = PerfilUsuario.Recepcao;

    // Só existe no formulário (não vai para o banco).
    [NotMapped]
    [DataType(DataType.Password)]
    [Display(Name = "Senha")]
    public string? SenhaNova { get; set; }

    // Usuario 1 --- N Matricula (registra) | Usuario 1 --- N FichaTreino (monta)
    public ICollection<Matricula> MatriculasRegistradas { get; set; } = new List<Matricula>();
    public ICollection<FichaTreino> FichasMontadas { get; set; } = new List<FichaTreino>();
}
