using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/[controller]")]
public class GitHubRepoController : ControllerBase
{
    private readonly IRepositoryService _repositoryService;

    public GitHubRepoController(IRepositoryService repositoryService)
    {
        _repositoryService = repositoryService;
    }

    [HttpGet("{owner}/{repoName}")]
    public async Task<IActionResult> GetRepository(string owner, string repoName)
    {
        // validations
        if (string.IsNullOrWhiteSpace(owner)|| string.IsNullOrWhiteSpace(repoName))
        {
            return BadRequest("Owner Name and Repository Name Are required");
        }

        try
        {
            var result = await _repositoryService.GetRepositoryAsync(owner,repoName);

            if(result == null) return NotFound();

            return Ok(result);
        }
        catch (HttpRequestException)
        {   
            return StatusCode(503,"External Github Service Unavailable.");
        }
        catch (Exception)
    {
            return StatusCode(500,"An unexpected error occurred.");
        }
    }
}