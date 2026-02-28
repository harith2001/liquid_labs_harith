using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations;

public class RepoRepository
{
    private readonly IDBConnectionFactory _connectionFactory;
    public RepoRepository(IDBConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Repository?> GetByFullNameAsync(string fullName)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        string query = @"
            SELECT rp.*, o.Id as OwnerId, o.GithubId as OwnerGithubId, o.Login, o.Url, o.Type
            FROM Repositories rp
            INNER JOIN Owners o ON rp.OwnerID = o.Id
            WHERE rp.FullName = @FullName";

        var command = connection.CreateCommand();
        command.CommandText = query;
        // paramterized query
        var parameter = command.CreateParameter();
        parameter.ParameterName = "@FullName";
        parameter.Value = fullName;
        command.Parameters.Add(parameter);

        using var reader = await command.ExecuteReaderAsync();

        // check validation
        if (!reader.HasRows)
            return null;

        await reader.ReadAsync();

        return new Repository
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            GithubId = reader.GetInt64(reader.GetOrdinal("GithubId")),
            Name = reader.GetString(reader.GetOrdinal("Name")),
            FullName = reader.GetString(reader.GetOrdinal("FullName")),
            Description = reader["Description"] as string,
            StargazersCount = reader.GetInt32(reader.GetOrdinal("StargazersCount")),
            ForksCount = reader.GetInt32(reader.GetOrdinal("ForksCount")),
            OpenIssuesCount = reader.GetInt32(reader.GetOrdinal("OpenIssuesCount")),
            Language = reader["Language"] as string,
            CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
            UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
            Owner = new Owner
            {
                Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                GithubId = reader.GetInt64(reader.GetOrdinal("OwnerGithubId")),
                Login = reader.GetString(reader.GetOrdinal("Login")),
                Url = reader["Url"] as string,
                Type = reader["Type"] as string
            }
        };
    }
}