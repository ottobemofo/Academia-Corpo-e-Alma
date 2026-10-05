namespace GestaoAcademia.Models;

public class AlunoFormViewModel
{
    public int IdPessoa { get; set; }

    public string Nome { get; set; } = "";

    public string Telefone { get; set; } = "";

    public int IdPlano { get; set; }

    public int DiaPagamento { get; set; }

    public DateTime? UltimoPagamento { get; set; }

    public int IdUsuario { get; set; }
}