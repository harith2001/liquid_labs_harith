public class RepositoryService : IRepositoryService
{
    private readonly RepoRepository _repositoryRepository;
    private readonly IGitHubService _gitHubService;

    public RepositoryService(
        RepoRepository repoRepository,
        IGitHubService gitHubService)
    {
        _repositoryRepository = repoRepository;
        _gitHubService = gitHubService;
    }

    public async Task<Repository?> GetRepositoryAsync(string owner, string repoName)
    {   
        // full name of the repo
        string fullName = $"{owner}/{repoName}";
        
        // check the data if it exist in DB
        var existRepo = await _repositoryRepository.GetByFullNameAsync(fullName);

        if(existRepo != null)
        {
            return existRepo; // returning cached data
        }

        // fetch from github
        var repoFromGit = await _gitHubService.GetRepositoryAsync(owner,repoName);

        if(repoFromGit == null)
        {
            return null;  // Not found
        }

        // save it in DB
        await _repositoryRepository.InsertAsync(repoFromGit);

        // return newly added cached 
        return repoFromGit;

    }
}