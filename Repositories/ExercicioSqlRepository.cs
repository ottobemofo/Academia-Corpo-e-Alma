using Microsoft.Data.SqlClient;
using GestaoAcademia.Models;

namespace GestaoAcademia.Repositories;

public class ExercicioSqlRepository : DataConnection, IExercicioRepository
{
    public void Create(Exercicio exercicio)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "INSERT INTO Exercicio (nome_exercicio, grupo_muscular, series, repeticoes, carga_kg) VALUES (@NomeExercicio, @GrupoMuscular, @Series, @Repeticoes, @CargaKg)";
        cmd.Connection = conn;
        cmd.Parameters.AddWithValue("@NomeExercicio", exercicio.NomeExercicio);
        cmd.Parameters.AddWithValue("@GrupoMuscular", exercicio.GrupoMuscular);
        cmd.Parameters.AddWithValue("@Series", exercicio.Series);
        cmd.Parameters.AddWithValue("@Repeticoes", exercicio.Repeticoes);
        cmd.Parameters.AddWithValue("@CargaKg", exercicio.CargaKg);
        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        SqlTransaction transaction = conn.BeginTransaction();

        try
        {
            SqlCommand fichaExercicioCmd = new SqlCommand();
            fichaExercicioCmd.CommandText = "DELETE FROM Ficha_Exercicio WHERE id_exercicio = @IdExercicio";
            fichaExercicioCmd.Connection = conn;
            fichaExercicioCmd.Transaction = transaction;
            fichaExercicioCmd.Parameters.AddWithValue("@IdExercicio", id);
            fichaExercicioCmd.ExecuteNonQuery();

            SqlCommand exercicioCmd = new SqlCommand();
            exercicioCmd.CommandText = "DELETE FROM Exercicio WHERE id_exercicio = @IdExercicio";
            exercicioCmd.Connection = conn;
            exercicioCmd.Transaction = transaction;
            exercicioCmd.Parameters.AddWithValue("@IdExercicio", id);
            exercicioCmd.ExecuteNonQuery();

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public List<Exercicio> Read()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT id_exercicio, nome_exercicio, grupo_muscular, series, repeticoes, carga_kg FROM Exercicio ORDER BY id_exercicio";
        cmd.Connection = conn;

        List<Exercicio> exercicios = new List<Exercicio>();

        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Exercicio exercicio = new Exercicio();
            exercicio.IdExercicio = reader.GetInt32(0);
            exercicio.NomeExercicio = reader.GetString(1);
            exercicio.GrupoMuscular = reader.GetString(2);
            exercicio.Series = reader.GetInt32(3);
            exercicio.Repeticoes = reader.GetInt32(4);
            exercicio.CargaKg = reader.GetDecimal(5);

            exercicios.Add(exercicio);
        }

        return exercicios;
    }

    public Exercicio Read(int id)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT id_exercicio, nome_exercicio, grupo_muscular, series, repeticoes, carga_kg FROM Exercicio WHERE id_exercicio = @IdExercicio";
        cmd.Connection = conn;
        cmd.Parameters.AddWithValue("@IdExercicio", id);

        using SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            Exercicio exercicio = new Exercicio();
            exercicio.IdExercicio = reader.GetInt32(0);
            exercicio.NomeExercicio = reader.GetString(1);
            exercicio.GrupoMuscular = reader.GetString(2);
            exercicio.Series = reader.GetInt32(3);
            exercicio.Repeticoes = reader.GetInt32(4);
            exercicio.CargaKg = reader.GetDecimal(5);

            return exercicio;
        }

        return null;
    }

    public void Update(Exercicio exercicio)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE Exercicio SET nome_exercicio = @NomeExercicio, grupo_muscular = @GrupoMuscular, series = @Series, repeticoes = @Repeticoes, carga_kg = @CargaKg WHERE id_exercicio = @IdExercicio";
        cmd.Connection = conn;
        cmd.Parameters.AddWithValue("@NomeExercicio", exercicio.NomeExercicio);
        cmd.Parameters.AddWithValue("@GrupoMuscular", exercicio.GrupoMuscular);
        cmd.Parameters.AddWithValue("@Series", exercicio.Series);
        cmd.Parameters.AddWithValue("@Repeticoes", exercicio.Repeticoes);
        cmd.Parameters.AddWithValue("@CargaKg", exercicio.CargaKg);
        cmd.Parameters.AddWithValue("@IdExercicio", exercicio.IdExercicio);
        cmd.ExecuteNonQuery();
    }
}
