using Microsoft.Data.SqlClient;
using GestaoAcademia.Models;

namespace GestaoAcademia.Repositories;

public class AlunoSqlRepository : DataConnection, IAlunoRepository
{
    public void Create(Aluno aluno)
    {
        SqlTransaction transaction = conn.BeginTransaction();

        try
        {
            SqlCommand pessoaCmd = new SqlCommand();
            //insere e retorna o valor do id gerado
            pessoaCmd.CommandText = "INSERT INTO Pessoa (nome) VALUES (@Nome); SELECT CAST(SCOPE_IDENTITY() AS INT);";
            pessoaCmd.Connection = conn;
            pessoaCmd.Transaction = transaction;
            pessoaCmd.Parameters.AddWithValue("@Nome", aluno.Nome);

            //retorna um único valor
            int idPessoa = (int)pessoaCmd.ExecuteScalar();

            SqlCommand alunoCmd = new SqlCommand();
            alunoCmd.CommandText = "INSERT INTO Aluno (id_aluno, telefone, status_aluno) VALUES (@IdAluno, @Telefone, @StatusAluno)";
            alunoCmd.Connection = conn;
            alunoCmd.Transaction = transaction;
            alunoCmd.Parameters.AddWithValue("@IdAluno", idPessoa);
            alunoCmd.Parameters.AddWithValue("@Telefone", aluno.Telefone);
            alunoCmd.Parameters.AddWithValue("@StatusAluno", aluno.StatusAluno.ToString().ToUpper());
            
             //não precisamos ler registros de volta.
            alunoCmd.ExecuteNonQuery();

            transaction.Commit();
            aluno.IdPessoa = idPessoa;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public void Delete(int id)
    {
        SqlTransaction transaction = conn.BeginTransaction();

        try
        {
            SqlCommand alunoCmd = new SqlCommand();
            alunoCmd.CommandText = "DELETE FROM Aluno WHERE id_aluno = @IdAluno";
            alunoCmd.Connection = conn;
            alunoCmd.Transaction = transaction;
            alunoCmd.Parameters.AddWithValue("@IdAluno", id);
            alunoCmd.ExecuteNonQuery();

            SqlCommand pessoaCmd = new SqlCommand();
            pessoaCmd.CommandText = "DELETE FROM Pessoa WHERE id_pessoa = @IdPessoa";
            pessoaCmd.Connection = conn;
            pessoaCmd.Transaction = transaction;
            pessoaCmd.Parameters.AddWithValue("@IdPessoa", id);
            pessoaCmd.ExecuteNonQuery();

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public List<Aluno> Read()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = @"
            SELECT p.id_pessoa, p.nome, a.telefone, a.status_aluno
            FROM Pessoa p
            INNER JOIN Aluno a ON a.id_aluno = p.id_pessoa
            ORDER BY p.id_pessoa";
        cmd.Connection = conn;

        List<Aluno> alunos = new List<Aluno>();

        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Aluno aluno = new Aluno();
            aluno.IdPessoa = reader.GetInt32(0);
            aluno.Nome = reader.GetString(1);
            aluno.Telefone = reader.GetString(2);
            aluno.StatusAluno = Enum.Parse<StatusAluno>(reader.GetString(3), true);

            alunos.Add(aluno);
        }

        return alunos;
    }

    public Aluno Read(int id)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = @"
            SELECT p.id_pessoa, p.nome, a.telefone, a.status_aluno
            FROM Pessoa p
            INNER JOIN Aluno a ON a.id_aluno = p.id_pessoa
            WHERE p.id_pessoa = @IdPessoa";
        cmd.Connection = conn;
        cmd.Parameters.AddWithValue("@IdPessoa", id);

        using SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            Aluno aluno = new Aluno();
            aluno.IdPessoa = reader.GetInt32(0);
            aluno.Nome = reader.GetString(1);
            aluno.Telefone = reader.GetString(2);
            aluno.StatusAluno = Enum.Parse<StatusAluno>(reader.GetString(3), true);

            return aluno;
        }

        return null;
    }

    public void Update(Aluno aluno)
    {
        SqlTransaction transaction = conn.BeginTransaction();

        try
        {
            SqlCommand pessoaCmd = new SqlCommand();
            pessoaCmd.CommandText = "UPDATE Pessoa SET nome = @Nome WHERE id_pessoa = @IdPessoa";
            pessoaCmd.Connection = conn;
            pessoaCmd.Transaction = transaction;
            pessoaCmd.Parameters.AddWithValue("@Nome", aluno.Nome);
            pessoaCmd.Parameters.AddWithValue("@IdPessoa", aluno.IdPessoa);
            pessoaCmd.ExecuteNonQuery();

            SqlCommand alunoCmd = new SqlCommand();
            alunoCmd.CommandText = "UPDATE Aluno SET telefone = @Telefone, status_aluno = @StatusAluno WHERE id_aluno = @IdAluno";
            alunoCmd.Connection = conn;
            alunoCmd.Transaction = transaction;
            alunoCmd.Parameters.AddWithValue("@Telefone", aluno.Telefone);
            alunoCmd.Parameters.AddWithValue("@StatusAluno", aluno.StatusAluno.ToString().ToUpper());
            alunoCmd.Parameters.AddWithValue("@IdAluno", aluno.IdPessoa);
            alunoCmd.ExecuteNonQuery();

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
