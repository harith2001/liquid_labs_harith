public interface IGitHubService
{
    Task<Repository?> GetRepositoryAsync (string owner, string repoName);
}