using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubIntegration.Entities
{
    public class RepositoryInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<RepositoryLanguage> Languages { get; set; }
        public DateTimeOffset LastCommit { get; set; }
        public int StargazersCount { get; set; }
        public int PullRequestCount { get; set; }
        public string HtmlUrl { get; set; }
    }
}
