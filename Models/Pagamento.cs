namespace GestaoAcademia.Models;

public class Pagamento
{
    public int IdPagamento { get; set; }

    public DateTime DataPagamento { get; set; }

    public string FormaPagamento { get; set; } = "";

    public int IdMatricula { get; set; }
}