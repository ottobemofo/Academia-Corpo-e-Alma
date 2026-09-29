using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoAcademia.Models;

public class Pagamento
{
    [Key]
    public int IdPagamento { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Data do pagamento")]
    public DateTime DataPagamento { get; set; } = DateTime.Today;

    [Column(TypeName = "decimal(10,2)")]
    [Range(0.01, 100000, ErrorMessage = "Informe um valor maior que zero.")]
    [Display(Name = "Valor pago (R$)")]
    public decimal ValorPago { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Selecione a matrícula.")]
    [Display(Name = "Matrícula")]
    public int MatriculaId { get; set; }
    public Matricula? Matricula { get; set; }
}
