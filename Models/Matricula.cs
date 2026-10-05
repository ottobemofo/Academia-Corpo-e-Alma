namespace GestaoAcademia.Models;

public class Matricula
{
    public int IdMatricula { get; set; }

    public DateTime DataMatricula { get; set; }

    public int DiaPagamento { get; set; }

    public int IdAluno { get; set; }

    public int IdPlano { get; set; }

    public int IdUsuario { get; set; }

    public DateTime? UltimoPagamento { get; set; }

    public int MesesEmAtraso { get; set; }
}