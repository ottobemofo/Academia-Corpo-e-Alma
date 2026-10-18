using Microsoft.Data.SqlClient;
using GestaoAcademia.Models;

namespace GestaoAcademia.Repositories;

public class FichaTreinoSqlRepository : DataConnection, IFichaTreinoRepository
{
    public int Create(FichaTreino fichaTreino)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.CommandText =
            @"INSERT INTO Ficha_Treino
            (id_aluno, id_usuario, nome_treino, data_criacao, objetivo)
            OUTPUT INSERTED.id_treino
            VALUES
            (@IdAluno, @IdUsuario, @NomeTreino, @DataCriacao, @Objetivo)";

        cmd.Connection = conn;

        cmd.Parameters.AddWithValue(
            "@IdAluno",
            fichaTreino.IdAluno
        );

        cmd.Parameters.AddWithValue(
            "@IdUsuario",
            fichaTreino.IdUsuario
        );

        cmd.Parameters.AddWithValue(
            "@NomeTreino",
            fichaTreino.NomeTreino
        );

        cmd.Parameters.AddWithValue(
            "@DataCriacao",
            fichaTreino.DataCriacao
        );

        cmd.Parameters.AddWithValue(
            "@Objetivo",
            fichaTreino.Objetivo
        );

        int idTreino = (int)cmd.ExecuteScalar()!;

        return idTreino;
    }


    public void Delete(int id)
    {
        SqlTransaction transaction =
            conn.BeginTransaction();

        try
        {
            SqlCommand fichaExercicioCmd =
                new SqlCommand();

            fichaExercicioCmd.CommandText =
                @"DELETE FROM Ficha_Exercicio
                WHERE id_treino = @IdTreino";

            fichaExercicioCmd.Connection = conn;

            fichaExercicioCmd.Transaction =
                transaction;

            fichaExercicioCmd.Parameters.AddWithValue(
                "@IdTreino",
                id
            );

            fichaExercicioCmd.ExecuteNonQuery();


            SqlCommand fichaCmd =
                new SqlCommand();

            fichaCmd.CommandText =
                @"DELETE FROM Ficha_Treino
                WHERE id_treino = @IdTreino";

            fichaCmd.Connection = conn;

            fichaCmd.Transaction =
                transaction;

            fichaCmd.Parameters.AddWithValue(
                "@IdTreino",
                id
            );

            fichaCmd.ExecuteNonQuery();


            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }


    public List<FichaTreino> Read()
    {
        SqlCommand cmd = new SqlCommand();

        cmd.CommandText =
            @"SELECT
                id_treino,
                nome_treino,
                data_criacao,
                objetivo,
                id_aluno,
                id_usuario
            FROM Ficha_Treino
            ORDER BY id_treino";

        cmd.Connection = conn;

        List<FichaTreino> fichas =
            new List<FichaTreino>();

        using SqlDataReader reader =
            cmd.ExecuteReader();

        while (reader.Read())
        {
            FichaTreino ficha =
                new FichaTreino();

            ficha.IdTreino =
                reader.GetInt32(0);

            ficha.NomeTreino =
                reader.GetString(1);

            ficha.DataCriacao =
                reader.GetDateTime(2);

            ficha.Objetivo =
                reader.GetString(3);

            ficha.IdAluno =
                reader.GetInt32(4);

            ficha.IdUsuario =
                reader.GetInt32(5);

            fichas.Add(ficha);
        }

        return fichas;
    }


    public FichaTreino Read(int id)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.CommandText =
            @"SELECT
                id_treino,
                nome_treino,
                data_criacao,
                objetivo,
                id_aluno,
                id_usuario
            FROM Ficha_Treino
            WHERE id_treino = @IdTreino";

        cmd.Connection = conn;

        cmd.Parameters.AddWithValue(
            "@IdTreino",
            id
        );

        using SqlDataReader reader =
            cmd.ExecuteReader();

        if (reader.Read())
        {
            FichaTreino ficha =
                new FichaTreino();

            ficha.IdTreino =
                reader.GetInt32(0);

            ficha.NomeTreino =
                reader.GetString(1);

            ficha.DataCriacao =
                reader.GetDateTime(2);

            ficha.Objetivo =
                reader.GetString(3);

            ficha.IdAluno =
                reader.GetInt32(4);

            ficha.IdUsuario =
                reader.GetInt32(5);

            return ficha;
        }

        return null;
    }


    public void Update(FichaTreino fichaTreino)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.CommandText =
            @"UPDATE Ficha_Treino
            SET
                id_aluno = @IdAluno,
                id_usuario = @IdUsuario,
                nome_treino = @NomeTreino,
                data_criacao = @DataCriacao,
                objetivo = @Objetivo
            WHERE id_treino = @IdTreino";

        cmd.Connection = conn;

        cmd.Parameters.AddWithValue(
            "@IdAluno",
            fichaTreino.IdAluno
        );

        cmd.Parameters.AddWithValue(
            "@IdUsuario",
            fichaTreino.IdUsuario
        );

        cmd.Parameters.AddWithValue(
            "@NomeTreino",
            fichaTreino.NomeTreino
        );

        cmd.Parameters.AddWithValue(
            "@DataCriacao",
            fichaTreino.DataCriacao
        );

        cmd.Parameters.AddWithValue(
            "@Objetivo",
            fichaTreino.Objetivo
        );

        cmd.Parameters.AddWithValue(
            "@IdTreino",
            fichaTreino.IdTreino
        );

        cmd.ExecuteNonQuery();
    }
}