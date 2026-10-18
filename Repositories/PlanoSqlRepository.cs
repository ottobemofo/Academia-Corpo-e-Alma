using Microsoft.Data.SqlClient;
using GestaoAcademia.Models;

namespace GestaoAcademia.Repositories;

public class PlanoSqlRepository : DataConnection, IPlanoRepository
{
    public void Create(Plano plano)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "INSERT INTO Plano (nome_plano, valor_plano) VALUES (@NomePlano, @ValorPlano)";
        cmd.Connection = conn;
        cmd.Parameters.AddWithValue("@NomePlano", plano.NomePlano);
        cmd.Parameters.AddWithValue("@ValorPlano", plano.ValorPlano);
        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "DELETE FROM Plano WHERE id_plano = @IdPlano";
        cmd.Connection = conn;
        cmd.Parameters.AddWithValue("@IdPlano", id);
        cmd.ExecuteNonQuery();
    }

    public List<Plano> Read()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT id_plano, nome_plano, valor_plano FROM Plano ORDER BY id_plano";
        cmd.Connection = conn;

        List<Plano> planos = new List<Plano>();

        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Plano plano = new Plano();
            plano.IdPlano = reader.GetInt32(0);
            plano.NomePlano = reader.GetString(1);
            plano.ValorPlano = reader.GetDecimal(2);

            planos.Add(plano);
        }

        return planos;
    }

    public Plano Read(int id)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT id_plano, nome_plano, valor_plano FROM Plano WHERE id_plano = @IdPlano";
        cmd.Connection = conn;
        cmd.Parameters.AddWithValue("@IdPlano", id);

        using SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            Plano plano = new Plano();
            plano.IdPlano = reader.GetInt32(0);
            plano.NomePlano = reader.GetString(1);
            plano.ValorPlano = reader.GetDecimal(2);

            return plano;
        }

        return null;
    }

    public void Update(Plano plano)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE Plano SET nome_plano = @NomePlano, valor_plano = @ValorPlano WHERE id_plano = @IdPlano";
        cmd.Connection = conn;
        cmd.Parameters.AddWithValue("@NomePlano", plano.NomePlano);
        cmd.Parameters.AddWithValue("@ValorPlano", plano.ValorPlano);
        cmd.Parameters.AddWithValue("@IdPlano", plano.IdPlano);
        cmd.ExecuteNonQuery();
    }
}
