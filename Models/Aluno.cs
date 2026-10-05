namespace GestaoAcademia.Models;

public class Aluno : Pessoa
{
    public string Telefone { get; set; } = "";

    public StatusAluno StatusAluno { get; set; } = StatusAluno.Ativo;
}