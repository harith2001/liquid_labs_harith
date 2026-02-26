using System.Data.SqlClient;

public interface IDBCOnnectionFactory
{
    SqlConnection CreateConnection();
}