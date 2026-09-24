using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoAcademia.Models;

public class FichaTreino
{
    [Key]
    public int IdTreino { get; set; }

    [Required(ErrorMessage = "Informe o nome do treino.")]
    [StringLength(100)]
    [Display(Name = "Nome do treino")]
    public string NomeTreino { get; set; } = "";

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Data de criação")]
    public DateTime DataCriacao { get; set; } = DateTime.Today;

    [StringLength(200)]
    [Display(Name = "Objetivo")]
    public string? Objetivo { get; set; }

    [Range(1, 20, ErrorMessage = "Séries entre 1 e 20.")]
    [Display(Name = "Séries")]
    public int Series { get; set; } = 3;

    [Range(1, 100, ErrorMessage = "Repetições entre 1 e 100.")]
    [Display(Name = "Repetições")]
    public int Repeticoes { get; set; } = 10;

    [Column(TypeName = "decimal(6,2)")]
    [Range(0, 1000, ErrorMessage = "Carga entre 0 e 1000 kg.")]
    [Display(Name = "Carga (kg)")]
    public decimal CargaKg { get; set; }

    // "possui": ficha pertence a um aluno
    [Range(1, int.MaxValue, ErrorMessage = "Selecione um aluno.")]
    [Display(Name = "Aluno")]
    public int AlunoId { get; set; }
    public Aluno? Aluno { get; set; }

    // "monta": usuário (instrutor) que montou a ficha
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    // "contem": N:N com Exercicio (o EF cria a tabela de junção sozinho)
    public ICollection<Exercicio> Exercicios { get; set; } = new List<Exercicio>();

    // Apoio ao formulário (multi-select). Não vai para o banco.
    [NotMapped]
    [Display(Name = "Exercícios")]
    public List<int> ExerciciosIds { get; set; } = new();
}
