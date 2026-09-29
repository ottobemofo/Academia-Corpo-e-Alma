using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoAcademia.Models;

public class Matricula
{
    [Key]
    public int IdMatricula { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Data da matrícula")]
    public DateTime DataMatricula { get; set; } = DateTime.Today;

    [Range(1, 28, ErrorMessage = "Informe um dia entre 1 e 28.")]
    [Display(Name = "Dia do pagamento")]
    public int DiaPagamento { get; set; } = 5;

    [StringLength(500)]
    [Display(Name = "Observação")]
    public string? Observacao { get; set; }

    // ---- Chaves estrangeiras + navegações ----
    [Range(1, int.MaxValue, ErrorMessage = "Selecione um aluno.")]
    [Display(Name = "Aluno")]
    public int AlunoId { get; set; }
    public Aluno? Aluno { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Selecione um plano.")]
    [Display(Name = "Plano")]
    public int PlanoId { get; set; }
    public Plano? Plano { get; set; }

    // Quem registrou a matrícula (relacionamento "registra")
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();

    // ---- Atributos DERIVADOS (elipses tracejadas no DER) ----
    // Não viram coluna: são calculados a partir dos pagamentos.
    // (lembre de fazer Include(m => m.Pagamentos) na consulta!)
    [NotMapped]
    [Display(Name = "Último pagamento")]
    public DateTime? UltimoPagamento =>
        Pagamentos.Count == 0 ? null : Pagamentos.Max(p => p.DataPagamento);

    [NotMapped]
    [Display(Name = "Meses em atraso")]
    public int MesesEmAtraso
    {
        get
        {
            var hoje = DateTime.Today;
            var referencia = UltimoPagamento ?? DataMatricula;
            int meses = (hoje.Year - referencia.Year) * 12 + hoje.Month - referencia.Month;
            // Se ainda não chegou o dia do pagamento neste mês, esse mês ainda não venceu.
            if (hoje.Day < DiaPagamento) meses--;
            return Math.Max(0, meses);
        }
    }
}
