using FluentAssertions;
using GithubUsersApi.Models;
using GithubUsersApi.Services;
using GithubUsersApi.Services.Clients;
using GithubUsersApi.Tests.Fixtures;
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
            githubClientFixture.InitGithubClient();
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

        [Fact]
        public async void Get_InputIsMoreThanTen_ReturnFirstTenUsers()
        {
            var githubService = new GithubService(_cacheService, _githubClient);

            var githubServiceMessage = await githubService.GetUsers(new List<string> { 
                "fromCache01", "user01", "user05", "user06",
                "user07", "user08", "user09", "user10", 
                "user11", "user12", "user13", "user14", "user15"
            });

            Assert.Equal(10, githubServiceMessage.Message.Count);
        }

        [Fact]
        public async void GetUser_SortByNameDescending()
        {
            var githubService = new GithubService(_cacheService, _githubClient);

            var githubServiceMessage = await githubService.GetUsers(new List<string>
            {
                "user01", "user05", "user06", "user07"
            });
            githubServiceMessage.Message.Should().BeInAscendingOrder(n => n.Name);
        }
    }
}
