using System.Text.Json;
using System.Net;
public class GitHubService : IGitHubService
{
    private readonly HttpClient _httpClient;

    public GitHubService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Repository?> GetRepositoryAsync(string owner, string repoName)
    {
        var response = await _httpClient.GetAsync($"repos/{owner}/{repoName}");

        if (response.StatusCode == HttpStatusCode.NotFound) return null;

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var dto = JsonSerializer.Deserialize<GithubRepositoryResponse>(content,options);
        if (dto == null) return null;

        return MapToDomain(dto);
    }

    private Repository MapToDomain(GithubRepositoryResponse dto)
    {
        return new Repository
        {
            GithubId = dto.Id,
            Name = dto.Name,
            FullName = dto.FullName,
            Description = dto.Description,
            StargazersCount = dto.StargazersCount,
            ForksCount = dto.ForksCount,
            OpenIssuesCount = dto.OpenIssuesCount,
            Language = dto.Language,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt,
            Owner = new Owner
            {
                GithubId = dto.Owner.Id,
                Login = dto.Owner.Login,
                Url= dto.Owner.Url,
                Type = dto.Owner.Type
            }
        };
    }
}