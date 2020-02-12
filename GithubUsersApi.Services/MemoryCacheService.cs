using GithubUsersApi.Models;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace GithubUsersApi.Services
{
    public class MemoryCacheService : ICacheService
    {
        private readonly int CacheExpirationMinutes = 2;

        private readonly IMemoryCache _memoryCache;

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public GithubUser GetGithubUser(string username)
        {
            return _memoryCache.Get<GithubUser>(username);
        }

        public void SetGithubUser(
            string username,
            GithubUser githubUser,
            MemoryCacheEntryOptions options = null
        )
        {
            if (options == null)
            {
                options = new MemoryCacheEntryOptions();
                options.AbsoluteExpiration = DateTime.Now.AddMinutes(CacheExpirationMinutes);
                options.Priority = CacheItemPriority.Normal;
            }

            _memoryCache.Set(username, githubUser, options);
        }
    }
}
