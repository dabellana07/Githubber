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

            var githubUsers = await githubService.GetUsers(new List<string> { "user01", "fromCache01" });

            Assert.Equal(2, githubUsers.Count);
        }

        [Fact]
        public async void GetUser_GithubUserNonExisting_ReturnWithEmptyList()
        {
            var githubService = new GithubService(_cacheService, _githubClient);

            var githubUsers = await githubService.GetUsers(new List<string> { "user02", "user03" });

            Assert.Empty(githubUsers);
        }

        [Fact]
        public async void GetUser_GithubUserWithNonExisting_ReturnAllUsersWithoutNonExisting()
        {
            var githubService = new GithubService(_cacheService, _githubClient);

            var githubUsers = await githubService.GetUsers(new List<string> { "fromCache01", "user03", "user04" });

            Assert.Single(githubUsers);
        }

        [Fact]
        public async void Get_InputIsMoreThanTen_ReturnFirstTenUsers()
        {
            var githubService = new GithubService(_cacheService, _githubClient);

            var githubUsers = await githubService.GetUsers(new List<string> { 
                "fromCache01", "user01", "user05", "user06",
                "user07", "user08", "user09", "user10", 
                "user11", "user12", "user13", "user14", "user15"
            });

            Assert.Equal(10, githubUsers.Count);
        }

        [Fact]
        public async void GetUser_SortByNameDescending()
        {
            var githubService = new GithubService(_cacheService, _githubClient);

            var githubUsers = await githubService.GetUsers(new List<string>
            {
                "user01", "user05", "user06", "user07"
            });
            githubUsers.Should().BeInAscendingOrder(n => n.Name);
        }

        [Fact]
        public void IsValidUsername_AlphanumericUsername_ReturnTrue()
        {
            var githubService = new GithubService(_cacheService, _githubClient);

            var username = "destiny07";

            var result = githubService.IsValidUsername(username);

            Assert.True(result);
        }

        [Fact]
        public void IsValidUsername_UsernameWithHyphen_ReturnTrue()
        {
            var githubService = new GithubService(_cacheService, _githubClient);

            var username = "random-user01";

            var result = githubService.IsValidUsername(username);

            Assert.True(result);
        }

        [Fact]
        public void IsValidUsername_UsernameStartsWithHyphen_ReturnFalse()
        {
            var githubService = new GithubService(_cacheService, _githubClient);

            var username = "-random-user01";

            var result = githubService.IsValidUsername(username);

            Assert.False(result);
        }

        [Fact]
        public void IsValidUsername_UsernameEndsWithHyphen_ReturnFalse()
        {
            var githubService = new GithubService(_cacheService, _githubClient);

            var username = "random-user01-";

            var result = githubService.IsValidUsername(username);

            Assert.False(result);
        }
    }
}
