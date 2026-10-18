using Microsoft.Data.SqlClient;
using GestaoAcademia.Models;

namespace GestaoAcademia.Repositories;

public class PagamentoSqlRepository : DataConnection, IPagamentoRepository
{
    public void Create(Pagamento pagamento)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "INSERT INTO Pagamento (id_matricula, data_pagamento, forma_pagamento) VALUES (@IdMatricula, @DataPagamento, @FormaPagamento)";
        cmd.Connection = conn;
        cmd.Parameters.AddWithValue("@IdMatricula", pagamento.IdMatricula);
        cmd.Parameters.AddWithValue("@DataPagamento", pagamento.DataPagamento);
        cmd.Parameters.AddWithValue("@FormaPagamento", pagamento.FormaPagamento);
        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "DELETE FROM Pagamento WHERE id_pagamento = @IdPagamento";
        cmd.Connection = conn;
        cmd.Parameters.AddWithValue("@IdPagamento", id);
        cmd.ExecuteNonQuery();
    }

    public List<Pagamento> Read()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT id_pagamento, data_pagamento, forma_pagamento, id_matricula FROM Pagamento ORDER BY id_pagamento";
        cmd.Connection = conn;

        List<Pagamento> pagamentos = new List<Pagamento>();

        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Pagamento pagamento = new Pagamento();
            pagamento.IdPagamento = reader.GetInt32(0);
            pagamento.DataPagamento = reader.GetDateTime(1);
            pagamento.FormaPagamento = reader.GetString(2);
            pagamento.IdMatricula = reader.GetInt32(3);

            pagamentos.Add(pagamento);
        }

        return pagamentos;
    }

    public Pagamento Read(int id)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT id_pagamento, data_pagamento, forma_pagamento, id_matricula FROM Pagamento WHERE id_pagamento = @IdPagamento";
        cmd.Connection = conn;
        cmd.Parameters.AddWithValue("@IdPagamento", id);

        using SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            Pagamento pagamento = new Pagamento();
            pagamento.IdPagamento = reader.GetInt32(0);
            pagamento.DataPagamento = reader.GetDateTime(1);
            pagamento.FormaPagamento = reader.GetString(2);
            pagamento.IdMatricula = reader.GetInt32(3);

            return pagamento;
        }

        return null;
    }

    public void Update(Pagamento pagamento)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "UPDATE Pagamento SET id_matricula = @IdMatricula, data_pagamento = @DataPagamento, forma_pagamento = @FormaPagamento WHERE id_pagamento = @IdPagamento";
        cmd.Connection = conn;
        cmd.Parameters.AddWithValue("@IdMatricula", pagamento.IdMatricula);
        cmd.Parameters.AddWithValue("@DataPagamento", pagamento.DataPagamento);
        cmd.Parameters.AddWithValue("@FormaPagamento", pagamento.FormaPagamento);
        cmd.Parameters.AddWithValue("@IdPagamento", pagamento.IdPagamento);
        cmd.ExecuteNonQuery();
    }
}
