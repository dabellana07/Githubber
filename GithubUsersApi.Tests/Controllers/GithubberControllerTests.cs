using GithubUsersApi.Controllers;
using GithubUsersApi.Messages;
using GithubUsersApi.Models;
using GithubUsersApi.Services;
using GithubUsersApi.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace GithubUsersApi.Tests.Controllers
{
    public class GithubberControllerTests : IClassFixture<GithubControllerTestFixture>
    {
        private GithubControllerTestFixture _githubControllerTestsFixture { get; set; }

        public GithubberControllerTests(GithubControllerTestFixture githubServiceFixture)
        {
            _githubControllerTestsFixture = githubServiceFixture;
        }

        [Fact]
        public async void Get_InputExisting_ReturnList()
        {
            var controller = new GithubberController(
                _githubControllerTestsFixture.CacheService,
                _githubControllerTestsFixture.GithubService
            );

            var usernames = new List<string>
            {
                "randomUser01",
                "randomUser02",
                "randomUser03"
            };

            var result = await controller.Get(usernames);

            Assert.Equal(3, result.Value.Count);
        }

        [Fact]
        public async void Get_InputWithNonExisting_ReturnList()
        {
            var controller = new GithubberController(
                _githubControllerTestsFixture.CacheService,
                _githubControllerTestsFixture.GithubService
            );

            var usernames = new List<string>
            {
                "randomUser01",
                "randomUser02",
                "randomUser03",
                "nonExistingUser01"
            };

            var result = await controller.Get(usernames);

            Assert.Equal(3, result.Value.Count);
        }

        [Fact]
        public async void Get_InputNonExisting_ReturnEmptyList()
        {
            var controller = new GithubberController(
                _githubControllerTestsFixture.CacheService,
                _githubControllerTestsFixture.GithubService
            );

            var usernames = new List<string>
            {
                "nonExistingUser01",
                "nonExistingUser02",
                "nonExistingUser03"
            };

            var result = await controller.Get(usernames);

            Assert.Empty(result.Value);
        }

        [Fact]
        public async void Get_InputIsEmpty_ReturnBadRequest()
        {
            var controller = new GithubberController(
                _githubControllerTestsFixture.CacheService,
                _githubControllerTestsFixture.GithubService
            );

            var result = await controller.Get(new List<string>());

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async void Get_InputIsMoreThanTen_ReturnBadRequest()
        {
            var controller = new GithubberController(
                _githubControllerTestsFixture.CacheService,
                _githubControllerTestsFixture.GithubService
            );

            var usernames = new List<string>
            {
                "Test1", "Test2", "Test3", "Test4", "Test5",
                "Test6", "Test7", "Test8", "Test9", "Test10",
                "Test11"
            };

            var result = await controller.Get(usernames);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async void Get_GetAUserFromCache_ReturnList()
        {
            var controller = new GithubberController(
                _githubControllerTestsFixture.CacheService,
                _githubControllerTestsFixture.GithubService
            );

            var usernames = new List<string>
            {
                "fromCache01", "randomUser01"
            };

            var result = await controller.Get(usernames);

            Assert.Equal(2, result.Value.Count);
        }
    }

    public class GithubControllerTestFixture
    {
        public GithubControllerTestFixture()
        {
            GithubService = InitGithubService();
            CacheService = InitCacheService();
        }

        private IGithubService InitGithubService()
        {
            var githubServiceMoq = new Mock<IGithubService>();
            githubServiceMoq.Setup(s => s.GetUser("randomUser01"))
                .ReturnsAsync(new GithubServiceMessage<GithubUser>(
                    new GithubUser
                    {
                        Name = "UserOne Random",
                        Login = "randomUser01",
                        Company = null,
                        PublicRepos = 3,
                        Followers = 10
                    }, null));
            githubServiceMoq.Setup(s => s.GetUser("randomUser02"))
                .ReturnsAsync(new GithubServiceMessage<GithubUser>(
                    new GithubUser
                    {
                        Name = "UserTwo Random",
                        Login = "randomUser02",
                        Company = "Random Company1",
                        PublicRepos = 1,
                        Followers = 1
                    }, null));
            githubServiceMoq.Setup(s => s.GetUser("randomUser03"))
                .ReturnsAsync(new GithubServiceMessage<GithubUser>(
                    new GithubUser
                    {
                        Name = "UserThree Random",
                        Login = "randomUser03",
                        Company = null,
                        PublicRepos = 3,
                        Followers = 1
                    }, null));
            githubServiceMoq.Setup(s => s.GetUser("nonExistingUser01"))
                .ReturnsAsync(new GithubServiceMessage<GithubUser>(
                    null, new Exception("Not Found")));
            githubServiceMoq.Setup(s => s.GetUser("nonExistingUser02"))
                .ReturnsAsync(new GithubServiceMessage<GithubUser>(
                    null, new Exception("Not Found")));
            githubServiceMoq.Setup(s => s.GetUser("nonExistingUser03"))
                .ReturnsAsync(new GithubServiceMessage<GithubUser>(
                    null, new Exception("Not Found")));
            return githubServiceMoq.Object;
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

        public IGithubService GithubService { get; set; }

        public ICacheService CacheService { get; set; }
    }
}
