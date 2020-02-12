using GithubUsersApi.Models;
using Microsoft.Extensions.Caching.Memory;

namespace GithubUsersApi.Services
{
    public interface ICacheService
    {
        void SetGithubUser(
            string username, 
            GithubUser githubUser,
            MemoryCacheEntryOptions options = null);
        GithubUser GetGithubUser(string username);
    }
}
