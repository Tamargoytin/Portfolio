using GitHubIntegration.Entities;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubIntegration
{
    public interface IGitHubService
    {
        public Task<List<RepositoryInfo>> GetMyRepositories();

        public Task<List<Repository>> SearchRepositories(string repositoryName = null, string language = null, string userName = null);
    }
}
