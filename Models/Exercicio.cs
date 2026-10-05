namespace GestaoAcademia.Models;

public class Exercicio
{
    public int IdExercicio { get; set; }

    public string NomeExercicio { get; set; } = "";

    public string GrupoMuscular { get; set; } = "";

    public int Series { get; set; }

    public int Repeticoes { get; set; }

    public decimal CargaKg { get; set; }
}