using Microsoft.Data.SqlClient;
using GestaoAcademia.Models;

namespace GestaoAcademia.Repositories;

public class FichaExercicioSqlRepository : DataConnection, IFichaExercicioRepository
{
    public void Create(FichaExercicio fichaExercicio)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "INSERT INTO Ficha_Exercicio (id_treino, id_exercicio) VALUES (@IdTreino, @IdExercicio)";
        cmd.Connection = conn;
        cmd.Parameters.AddWithValue("@IdTreino", fichaExercicio.IdTreino);
        cmd.Parameters.AddWithValue("@IdExercicio", fichaExercicio.IdExercicio);
        cmd.ExecuteNonQuery();
    }

    public void Delete(int idTreino, int idExercicio)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "DELETE FROM Ficha_Exercicio WHERE id_treino = @IdTreino AND id_exercicio = @IdExercicio";
        cmd.Connection = conn;
        cmd.Parameters.AddWithValue("@IdTreino", idTreino);
        cmd.Parameters.AddWithValue("@IdExercicio", idExercicio);
        cmd.ExecuteNonQuery();
    }

    public List<FichaExercicio> Read()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT id_treino, id_exercicio FROM Ficha_Exercicio ORDER BY id_treino, id_exercicio";
        cmd.Connection = conn;

        List<FichaExercicio> itens = new List<FichaExercicio>();

        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            FichaExercicio item = new FichaExercicio();
            item.IdTreino = reader.GetInt32(0);
            item.IdExercicio = reader.GetInt32(1);
            itens.Add(item);
        }

        return itens;
    }

    public List<FichaExercicio> ReadByTreino(int idTreino)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT id_treino, id_exercicio FROM Ficha_Exercicio WHERE id_treino = @IdTreino ORDER BY id_exercicio";
        cmd.Connection = conn;
        cmd.Parameters.AddWithValue("@IdTreino", idTreino);

        List<FichaExercicio> itens = new List<FichaExercicio>();

        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            FichaExercicio item = new FichaExercicio();
            item.IdTreino = reader.GetInt32(0);
            item.IdExercicio = reader.GetInt32(1);
            itens.Add(item);
        }

        return itens;
    }
}
