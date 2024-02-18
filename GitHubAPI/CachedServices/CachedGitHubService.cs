using GitHubIntegration;
using GitHubIntegration.Entities;
using Microsoft.Extensions.Caching.Memory;
using Octokit;

namespace GitHubAPI.CachedServices
{
    public class CachedGitHubService : IGitHubService
    {
        private readonly IGitHubService _gitHubService;
        private readonly IMemoryCache _memoryCache;

        private const string PortfolioKey = "PortfolioKey";

        public CachedGitHubService(IGitHubService gitHubService, IMemoryCache memoryCache)
        {
            _gitHubService = gitHubService; 
            _memoryCache = memoryCache; 
        }
        public async Task<List<RepositoryInfo>> GetMyRepositories()
        {
            if (_memoryCache.TryGetValue(PortfolioKey, out List<RepositoryInfo> repositoryInfo))
                return repositoryInfo;

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(30))
                .SetSlidingExpiration(TimeSpan.FromSeconds(10));

            repositoryInfo = await _gitHubService.GetMyRepositories();
            _memoryCache.Set(PortfolioKey, repositoryInfo, cacheOptions); 
            return repositoryInfo;
        }

        public Task<List<Repository>> SearchRepositories(string repositoryName = null, string language = null, string userName = null)
        {
            return _gitHubService.SearchRepositories(repositoryName, language, userName);  
        }
    }
}
