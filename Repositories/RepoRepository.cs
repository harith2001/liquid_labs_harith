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

    private static void AddParameter(DbCommand command, string name, object? value)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value ?? DBNull.Value;
        command.Parameters.Add(parameter);
    }

    private async Task<int> EnsureOwnerAsync(Owner Owner, DbConnection connection, DbTransaction transaction)
    {
        // check if the owner already exists
        string selectQuery = "SELECT Id FROM Owners WHERE GithubId = @GithubId";
        await using var selectCommand = connection.CreateCommand();
        selectCommand.CommandText = selectQuery;
        selectCommand.Transaction = transaction;
        AddParameter(selectCommand, "@GithubId", Owner.GithubId);

        var result = await selectCommand.ExecuteScalarAsync();
        if (result != null && int.TryParse(result.ToString(), out int existingOwnerId))
        {
            return existingOwnerId;
        }

        // insert new owner if not exists
        string insertQuery = @"
            INSERT INTO Owners (GithubId, Login, Url, Type)
            VALUES (@GithubId, @Login, @Url, @Type);
            SELECT SCOPE_IDENTITY();";

        await using var insertCommand = connection.CreateCommand();
        insertCommand.CommandText = insertQuery;
        insertCommand.Transaction = transaction;

        AddParameter(insertCommand, "@GithubId", Owner.GithubId);
        AddParameter(insertCommand, "@Login", Owner.Login);
        AddParameter(insertCommand, "@Url", Owner.Url);
        AddParameter(insertCommand, "@Type", Owner.Type);

        var newOwnerIdObj = await insertCommand.ExecuteScalarAsync();
        return Convert.ToInt32(newOwnerIdObj);
    }

    // GET - get by full name
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

    // POST - Insert Records
    public async Task InsertAsync(Repository repository)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        // using transaction to make sure the insert correctly
        using var transaction = connection.BeginTransaction();

        try
        {
            int ownerId = await EnsureOwnerAsync(repository.Owner, connection, transaction);

            string insertQuery= @"
            INSERT INTO Repositories
            (GithubId, Name, FullName, Description, StragazersCount, ForksCount, OpenIssuesCount, Language, CreatedAt, UpdatedAt, OwnerId)
            VALUES
            (@GithubId, @Name, @FullName, @Description, @StragazersCount, @ForksCount, @OpenIssuesCount, @Language, @CreatedAt, @UpdatedAt, @OwnerId)";

            await using var command = connection.CreateCommand();
            command.CommandText = insertQuery;
            command.Transaction = transaction;

            AddParameter(command, "@GithubId", repository.GithubId);
            AddParameter(command, "@Name", repository.Name);
            AddParameter(command, "@FullName", repository.FullName);
            AddParameter(command, "@Description", repository.Description);
            AddParameter(command, "@StragazersCount", repository.StargazersCount);
            AddParameter(command, "@ForksCount", repository.ForksCount);
            AddParameter(command, "@OpenIssuesCount", repository.OpenIssuesCount);
            AddParameter(command, "@Language", repository.Language);
            AddParameter(command, "@CreatedAt", repository.CreatedAt);
            AddParameter(command, "@UpdatedAt", repository.UpdatedAt);
            AddParameter(command, "@OwnerId", ownerId);

            await command.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
        }
        catch (System.Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}