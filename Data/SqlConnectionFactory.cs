using Microsoft.Data.SqlClient;
using System.Data.Common;

public class SqlConnectionFactory : IDBConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}