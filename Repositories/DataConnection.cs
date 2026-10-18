using Microsoft.Data.SqlClient;

namespace GestaoAcademia.Repositories;

public abstract class DataConnection
{
    protected SqlConnection conn;

    public DataConnection()
    {
        string strConn = @"Server=localhost;
Database=ACADEMIACORPOALMA;
Trusted_Connection=True;
TrustServerCertificate=True";

        conn = new SqlConnection(strConn);
        conn.Open();
    }

    public void Dispose()
    {
        conn.Close();
    }
}
