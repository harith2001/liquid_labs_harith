public interface IRepositoryService
{
    Task<Repository?> GetRepositoryAsync(string owner, string repoName);
    Task <List<Repository>> GetAllRepositoriesAsync();
}