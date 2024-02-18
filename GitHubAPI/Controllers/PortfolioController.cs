using GitHubIntegration;
using GitHubIntegration.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Octokit;

namespace GitHubAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PortfolioController : ControllerBase
    {
        private readonly IGitHubService _gitHubService;
        public PortfolioController(IGitHubService gitHubService )
        {
            _gitHubService = gitHubService;
        }

        [HttpGet("portfolio")]
        public async Task<List<RepositoryInfo>> GetPortfolio()
        {
            List<RepositoryInfo> list = await _gitHubService.GetMyRepositories();
            //foreach (RepositoryInfo item in list)
            //{
            //    await Console.Out.WriteLineAsync($"repo name: {item.Name}, Description: {item.Description}, LastCommit: {item.LastCommit}, " +
            //        $"StargazersCount: {item.StargazersCount}, PullRequestCount: {item.PullRequestCount}, Url: {item.HtmlUrl}");
            //}
            return list;
        }
        [HttpGet("search")]
        public async Task<List<Repository>> SearchRepositories(string repositoryName = null, string language = null, string userName = null) 
        {
            List< Repository> list = await _gitHubService.SearchRepositories(repositoryName, language, userName);
            //foreach (var repo in list)
            //{
            //    await Console.Out.WriteLineAsync($"repo name: {repo.Name}");
            //}
            return list;   

        }
    }
}
