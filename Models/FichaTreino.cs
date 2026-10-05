namespace GestaoAcademia.Models;

public class FichaTreino
{
    public int IdTreino { get; set; }

    public string NomeTreino { get; set; } = "";

    public DateTime DataCriacao { get; set; }

    public string Objetivo { get; set; } = "";

    public int IdAluno { get; set; }

    public int IdUsuario { get; set; }
}