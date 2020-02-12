using GithubUsersApi.Services;
using GithubUsersApi.Tests.Helpers;
using Xunit;

namespace GithubUsersApi.Tests.ApiServices
{
    public class CacheServiceTests
    {
        [Fact]
        public void GetGithubUser_InCache_ReturnGithubUser()
        {
            var memoryCacheFake = new PassiveMemoryCacheFake();

            var cacheService = new MemoryCacheService(memoryCacheFake);

            var githubUser = cacheService.GetGithubUser("fromCache01");

            Assert.NotNull(githubUser);
        }

        [Fact]
        public void GetGithubUser_NotInCache_ReturnNull()
        {
            var memoryCacheFake = new PassiveMemoryCacheFake();

            var cacheService = new MemoryCacheService(memoryCacheFake);

            var githubUser = cacheService.GetGithubUser("notInCacheUser");

            Assert.Null(githubUser);
        }
    }
}
