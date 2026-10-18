using Microsoft.Data.SqlClient;
using GestaoAcademia.Models;

namespace GestaoAcademia.Repositories;

public class UsuarioSqlRepository : DataConnection, IUsuarioRepository
{
    public void Create(Usuario usuario)
    {
        SqlTransaction transaction = conn.BeginTransaction();

        try
        {
            SqlCommand pessoaCmd = new SqlCommand();
            pessoaCmd.CommandText = "INSERT INTO Pessoa (nome) VALUES (@Nome); SELECT CAST(SCOPE_IDENTITY() AS INT);";
            pessoaCmd.Connection = conn;
            pessoaCmd.Transaction = transaction;
            pessoaCmd.Parameters.AddWithValue("@Nome", usuario.Nome);

            int idPessoa = (int)pessoaCmd.ExecuteScalar();

            SqlCommand usuarioCmd = new SqlCommand();
            usuarioCmd.CommandText = "INSERT INTO Usuario (id_usuario, email, senha, perfil) VALUES (@IdUsuario, @Email, @Senha, @Perfil)";
            usuarioCmd.Connection = conn;
            usuarioCmd.Transaction = transaction;
            usuarioCmd.Parameters.AddWithValue("@IdUsuario", idPessoa);
            usuarioCmd.Parameters.AddWithValue("@Email", usuario.Email);
            usuarioCmd.Parameters.AddWithValue("@Senha", usuario.Senha);
            usuarioCmd.Parameters.AddWithValue("@Perfil", usuario.Perfil.ToString().ToUpper());

            usuarioCmd.ExecuteNonQuery();

            transaction.Commit();
            usuario.IdPessoa = idPessoa;
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
            SqlCommand usuarioCmd = new SqlCommand();
            usuarioCmd.CommandText = "DELETE FROM Usuario WHERE id_usuario = @IdUsuario";
            usuarioCmd.Connection = conn;
            usuarioCmd.Transaction = transaction;
            usuarioCmd.Parameters.AddWithValue("@IdUsuario", id);
            usuarioCmd.ExecuteNonQuery();

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

    public List<Usuario> Read()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = @"
            SELECT p.id_pessoa, p.nome, u.email, u.senha, u.perfil
            FROM Pessoa p
            INNER JOIN Usuario u ON u.id_usuario = p.id_pessoa
            ORDER BY p.id_pessoa";
        cmd.Connection = conn;

        List<Usuario> usuarios = new List<Usuario>();

        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Usuario usuario = new Usuario();
            usuario.IdPessoa = reader.GetInt32(0);
            usuario.Nome = reader.GetString(1);
            usuario.Email = reader.GetString(2);
            usuario.Senha = reader.GetString(3);
            usuario.Perfil = Enum.Parse<PerfilUsuario>(reader.GetString(4), true);

            usuarios.Add(usuario);
        }

        return usuarios;
    }

    public Usuario Read(int id)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = @"
            SELECT p.id_pessoa, p.nome, u.email, u.senha, u.perfil
            FROM Pessoa p
            INNER JOIN Usuario u ON u.id_usuario = p.id_pessoa
            WHERE p.id_pessoa = @IdPessoa";
        cmd.Connection = conn;
        cmd.Parameters.AddWithValue("@IdPessoa", id);

        using SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            Usuario usuario = new Usuario();
            usuario.IdPessoa = reader.GetInt32(0);
            usuario.Nome = reader.GetString(1);
            usuario.Email = reader.GetString(2);
            usuario.Senha = reader.GetString(3);
            usuario.Perfil = Enum.Parse<PerfilUsuario>(reader.GetString(4), true);

            return usuario;
        }

        return null;
    }

    public void Update(Usuario usuario)
    {
        SqlTransaction transaction = conn.BeginTransaction();

        try
        {
            SqlCommand pessoaCmd = new SqlCommand();
            pessoaCmd.CommandText = "UPDATE Pessoa SET nome = @Nome WHERE id_pessoa = @IdPessoa";
            pessoaCmd.Connection = conn;
            pessoaCmd.Transaction = transaction;
            pessoaCmd.Parameters.AddWithValue("@Nome", usuario.Nome);
            pessoaCmd.Parameters.AddWithValue("@IdPessoa", usuario.IdPessoa);
            pessoaCmd.ExecuteNonQuery();

            SqlCommand usuarioCmd = new SqlCommand();
            usuarioCmd.CommandText = "UPDATE Usuario SET email = @Email, senha = @Senha, perfil = @Perfil WHERE id_usuario = @IdUsuario";
            usuarioCmd.Connection = conn;
            usuarioCmd.Transaction = transaction;
            usuarioCmd.Parameters.AddWithValue("@Email", usuario.Email);
            usuarioCmd.Parameters.AddWithValue("@Senha", usuario.Senha);
            usuarioCmd.Parameters.AddWithValue("@Perfil", usuario.Perfil.ToString().ToUpper());
            usuarioCmd.Parameters.AddWithValue("@IdUsuario", usuario.IdPessoa);
            usuarioCmd.ExecuteNonQuery();

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
