using GithubUsersApi.Models;
using GithubUsersApi.Services;
using GithubUsersApi.Services.Clients;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace GithubUsersApi.Tests.Services
{
    public class GithubServiceTests : IClassFixture<GithubServiceTestsFixture>
    {
        private readonly ICacheService _cacheService;
        private readonly IGithubClient _githubClient;

        public GithubServiceTests(GithubServiceTestsFixture githubClientFixture)
        {
            _cacheService = githubClientFixture.CacheService;
            _githubClient = githubClientFixture.GithubClient;
        }

        [Fact]
        public async void GetUser_GithubUserAllExisting_ReturnWithNonEmptyList()
        {
            var githubService = new GithubService(_cacheService, _githubClient);

            var githubServiceMessage = await githubService.GetUsers(new List<string> { "user01", "fromCache01" });

            Assert.Equal(2, githubServiceMessage.Message.Count);
        }

        [Fact]
        public async void GetUser_GithubUserNonExisting_ReturnWithEmptyList()
        {
            var githubService = new GithubService(_cacheService, _githubClient);

            var githubServiceMessage = await githubService.GetUsers(new List<string> { "user02", "user03" });

            Assert.Empty(githubServiceMessage.Message);
        }

        [Fact]
        public async void GetUser_GithubUserWithNonExisting_ReturnAllUsersWithoutNonExisting()
        {
            var githubService = new GithubService(_cacheService, _githubClient);

            var githubServiceMessage = await githubService.GetUsers(new List<string> { "fromCache01", "user03", "user04" });

            Assert.Single(githubServiceMessage.Message);
        }
    }

    public class GithubServiceTestsFixture
    {
        public GithubServiceTestsFixture()
        {
            GithubClient = InitGithubClient();
            CacheService = InitCacheService();
        }

        private IGithubClient InitGithubClient()
        {
            var githubClientMoq = new Mock<IGithubClient>();
            githubClientMoq.Setup(s => s.GetUserByLogin("user01"))
                .ReturnsAsync(new GithubUser
                {
                    Name = "User One",
                    Login = "user01",
                    Company = null,
                    PublicRepos = 3,
                    Followers = 10
                });
            githubClientMoq.Setup(s => s.GetUserByLogin("user02"))
                .ReturnsAsync((GithubUser)null);
            return githubClientMoq.Object;
        }

        private ICacheService InitCacheService()
        {
            var cacheServiceMoq = new Mock<ICacheService>();
            cacheServiceMoq
                .Setup(s => s.GetGithubUser("fromCache01"))
                .Returns(new GithubUser
                {
                    Name = "From CacheOne",
                    Login = "fromCache01",
                    Company = null,
                    PublicRepos = 1,
                    Followers = 1
                });
            cacheServiceMoq
                .Setup(s => s.GetGithubUser("randomUser01"))
                .Returns((GithubUser)null);
            cacheServiceMoq
                .Setup(s => s.GetGithubUser("randomUser02"))
                .Returns((GithubUser)null);
            cacheServiceMoq
                .Setup(s => s.GetGithubUser("randomUser02"))
                .Returns((GithubUser)null);
            return cacheServiceMoq.Object;
        }

        public IGithubClient GithubClient { get; set; }

        public ICacheService CacheService { get; set; }
    }
}
