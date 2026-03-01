public interface IRepositoryService
{
    Task<Repository?> GetRepositoryAsync(string owner, string repoName);
}