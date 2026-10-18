using Microsoft.Data.SqlClient;
using GestaoAcademia.Models;

namespace GestaoAcademia.Repositories;

public class MatriculaSqlRepository : DataConnection, IMatriculaRepository
{
    public void Create(Matricula matricula)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = @"INSERT INTO Matricula
            (id_aluno, id_plano, id_usuario, data_matricula, dia_pagamento, ultimo_pagamento, meses_em_atraso)
            VALUES
            (@IdAluno, @IdPlano, @IdUsuario, @DataMatricula, @DiaPagamento, @UltimoPagamento, @MesesEmAtraso)";
        cmd.Connection = conn;

        cmd.Parameters.AddWithValue("@IdAluno", matricula.IdAluno);
        cmd.Parameters.AddWithValue("@IdPlano", matricula.IdPlano);
        cmd.Parameters.AddWithValue("@IdUsuario", matricula.IdUsuario);
        cmd.Parameters.AddWithValue("@DataMatricula", matricula.DataMatricula);
        cmd.Parameters.AddWithValue("@DiaPagamento", matricula.DiaPagamento);
        cmd.Parameters.AddWithValue("@UltimoPagamento", matricula.UltimoPagamento.HasValue ? (object)matricula.UltimoPagamento.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("@MesesEmAtraso", matricula.MesesEmAtraso);

        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        SqlTransaction transaction = conn.BeginTransaction();

        try
        {
            SqlCommand pagamentoCmd = new SqlCommand();
            pagamentoCmd.CommandText = "DELETE FROM Pagamento WHERE id_matricula = @IdMatricula";
            pagamentoCmd.Connection = conn;
            pagamentoCmd.Transaction = transaction;
            pagamentoCmd.Parameters.AddWithValue("@IdMatricula", id);
            pagamentoCmd.ExecuteNonQuery();

            SqlCommand matriculaCmd = new SqlCommand();
            matriculaCmd.CommandText = "DELETE FROM Matricula WHERE id_matricula = @IdMatricula";
            matriculaCmd.Connection = conn;
            matriculaCmd.Transaction = transaction;
            matriculaCmd.Parameters.AddWithValue("@IdMatricula", id);
            matriculaCmd.ExecuteNonQuery();

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public List<Matricula> Read()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = @"SELECT id_matricula, data_matricula, dia_pagamento, id_aluno, id_plano, id_usuario, ultimo_pagamento, meses_em_atraso
                            FROM Matricula
                            ORDER BY id_matricula";
        cmd.Connection = conn;

        List<Matricula> matriculas = new List<Matricula>();

        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Matricula matricula = new Matricula();
            matricula.IdMatricula = reader.GetInt32(0);
            matricula.DataMatricula = reader.GetDateTime(1);
            matricula.DiaPagamento = reader.GetInt32(2);
            matricula.IdAluno = reader.GetInt32(3);
            matricula.IdPlano = reader.GetInt32(4);
            matricula.IdUsuario = reader.GetInt32(5);
            matricula.UltimoPagamento = reader.IsDBNull(6) ? null : reader.GetDateTime(6);
            matricula.MesesEmAtraso = reader.GetInt32(7);

            matriculas.Add(matricula);
        }

        return matriculas;
    }

    public Matricula Read(int id)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = @"SELECT id_matricula, data_matricula, dia_pagamento, id_aluno, id_plano, id_usuario, ultimo_pagamento, meses_em_atraso
                            FROM Matricula
                            WHERE id_matricula = @IdMatricula";
        cmd.Connection = conn;
        cmd.Parameters.AddWithValue("@IdMatricula", id);

        using SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            Matricula matricula = new Matricula();
            matricula.IdMatricula = reader.GetInt32(0);
            matricula.DataMatricula = reader.GetDateTime(1);
            matricula.DiaPagamento = reader.GetInt32(2);
            matricula.IdAluno = reader.GetInt32(3);
            matricula.IdPlano = reader.GetInt32(4);
            matricula.IdUsuario = reader.GetInt32(5);
            matricula.UltimoPagamento = reader.IsDBNull(6) ? null : reader.GetDateTime(6);
            matricula.MesesEmAtraso = reader.GetInt32(7);

            return matricula;
        }

        return null;
    }

    public void Update(Matricula matricula)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = @"UPDATE Matricula SET
            id_aluno = @IdAluno,
            id_plano = @IdPlano,
            id_usuario = @IdUsuario,
            data_matricula = @DataMatricula,
            dia_pagamento = @DiaPagamento,
            ultimo_pagamento = @UltimoPagamento,
            meses_em_atraso = @MesesEmAtraso
            WHERE id_matricula = @IdMatricula";
        cmd.Connection = conn;

        cmd.Parameters.AddWithValue("@IdAluno", matricula.IdAluno);
        cmd.Parameters.AddWithValue("@IdPlano", matricula.IdPlano);
        cmd.Parameters.AddWithValue("@IdUsuario", matricula.IdUsuario);
        cmd.Parameters.AddWithValue("@DataMatricula", matricula.DataMatricula);
        cmd.Parameters.AddWithValue("@DiaPagamento", matricula.DiaPagamento);
        cmd.Parameters.AddWithValue("@UltimoPagamento", matricula.UltimoPagamento.HasValue ? (object)matricula.UltimoPagamento.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("@MesesEmAtraso", matricula.MesesEmAtraso);
        cmd.Parameters.AddWithValue("@IdMatricula", matricula.IdMatricula);

        cmd.ExecuteNonQuery();
    }
}
