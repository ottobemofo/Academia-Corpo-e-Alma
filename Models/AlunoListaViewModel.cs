namespace GestaoAcademia.Models;

public class AlunoListaViewModel
{
    public int IdPessoa { get; set; }

    public string Nome { get; set; } = "";

    public string Telefone { get; set; } = "";

    public string Plano { get; set; } = "";

    public DateTime DataMatricula { get; set; }

    public int DiaPagamento { get; set; }

    public DateTime? UltimoPagamento { get; set; }

    public int MesesEmAtraso { get; set; }

    public StatusAluno StatusAluno { get; set; }

    public string Funcionario { get; set; } = "";
}       