using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GitHubIntegration.Entities;
using Octokit;
using Microsoft.Extensions.Options;

namespace GitHubIntegration
{
    public class GitHubService: IGitHubService
    {
        private readonly GitHubClient _client;
        private readonly GitHubIntegrationOptions _options;
        public GitHubService(IOptions<GitHubIntegrationOptions> options)
        {
            _client = new GitHubClient(new ProductHeaderValue("my-github-app"));
            _options = options.Value;
            _client.Credentials = new Credentials(_options.Token);
        }  
        public async Task<List<RepositoryInfo>> GetMyRepositories()
        {
            //var githubClient = new GitHubClient(new ProductHeaderValue("MyGitHubApp"));
            //githubClient.Credentials = new Credentials(token);
            var repositories = await _client.Repository.GetAllForUser(_options.UserName);

            var result = new List<RepositoryInfo>();
            foreach (var repo in repositories)
            {
                var languages = await _client.Repository.GetAllLanguages(_options.UserName, repo.Name);
                var commits = await _client.Repository.Commit.GetAll(_options.UserName, repo.Name);
                var stars = await _client.Activity.Starring.GetAllStargazers(_options.UserName, repo.Name);
                var pullRequests = await _client.PullRequest.GetAllForRepository(_options.UserName, repo.Name);


                var repoInfo = new RepositoryInfo
                {
                    Name = repo.Name,
                    Description = repo.Description,
                    Languages = languages.ToList(),
                    LastCommit = commits.FirstOrDefault()?.Commit.Committer.Date ?? DateTimeOffset.MinValue,
                    StargazersCount = stars.Count,
                    PullRequestCount = pullRequests.Count,
                    HtmlUrl = repo.HtmlUrl
                };

                result.Add(repoInfo);
            }

            return result;
        }

        public async Task<List<Repository>> SearchRepositories(string repositoryName = null, string language=null, string userName = null)
        {

            if (!Enum.TryParse<Language>(language, true, out var languageEnum))
            {
                throw new Exception("error: languge is incorrect");          
            }
            // Set up the search request
            var searchRequest = new SearchRepositoriesRequest(repositoryName)
            {
                Language = languageEnum,
                User = userName,
            };

            // Perform the search
            var result = await _client.Search.SearchRepo(searchRequest);

            return result.Items.ToList();
        }



}
}
