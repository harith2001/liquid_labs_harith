using System.Data.Common;

public interface IDBConnectionFactory
{
    DbConnection CreateConnection();
}