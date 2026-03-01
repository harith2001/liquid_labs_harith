public class GithubRepositoryResponse
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string FullName { get; set; }
    public string? Description { get; set; }
    public int StargazersCount { get; set; }
    public int ForksCount { get; set; }
    public int OpenIssuesCount { get; set; }
    public string? Language { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public GitHubOwner Owner { get; set; }
}

public class GitHubOwner
{
    public long Id { get; set; }
    public string Login { get; set; }
    public string Url { get; set; }
    public string Type { get; set; }
}