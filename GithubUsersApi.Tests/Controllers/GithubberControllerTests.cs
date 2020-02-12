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
        private IGithubService _githubService { get; set; }

        public GithubberControllerTests(GithubControllerTestFixture githubServiceFixture)
        {
            _githubService = githubServiceFixture.GithubService;
        }

        [Fact]
        public async void Get_InputExisting_ReturnList()
        {
            var controller = new GithubberController(_githubService);

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
            var controller = new GithubberController(_githubService);

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
            var controller = new GithubberController(_githubService);

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
            var controller = new GithubberController(_githubService);

            var result = await controller.Get(new List<string>());

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async void Get_InputIsMoreThanTen_ReturnBadRequest()
        {
            var controller = new GithubberController(_githubService);

            var usernames = new List<string>
            {
                "Test1", "Test2", "Test3", "Test4", "Test5",
                "Test6", "Test7", "Test8", "Test9", "Test10",
                "Test11"
            };

            var result = await controller.Get(usernames);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
    }

    public class GithubControllerTestFixture
    {
        public GithubControllerTestFixture()
        {
            GithubService = InitGithubService();
        }

        private IGithubService InitGithubService()
        {
            var users = new List<GithubUser>()
            {
                new GithubUser
                {
                    Name = "UserOne Random",
                    Login = "randomUser01",
                    Company = null,
                    PublicRepos = 3,
                    Followers = 10
                },
                new GithubUser
                {
                    Name = "UserTwo Random",
                    Login = "randomUser02",
                    Company = "Random Company1",
                    PublicRepos = 1,
                    Followers = 1
                },
                new GithubUser
                {
                    Name = "UserThree Random",
                    Login = "randomUser03",
                    Company = null,
                    PublicRepos = 3,
                    Followers = 1
                }
            };
            var githubServiceMoq = new Mock<IGithubService>();
            githubServiceMoq.Setup(s => s.GetUsers(new List<string> { "randomUser01", "randomUser02", "randomUser03" }))
                .ReturnsAsync(new GithubServiceMessage<List<GithubUser>>(
                    users, null));
            githubServiceMoq.Setup(s => s.GetUsers(new List<string> {
                "randomUser01",
                "randomUser02",
                "randomUser03",
                "nonExistingUser01"
            })).ReturnsAsync(new GithubServiceMessage<List<GithubUser>>(users, null));
            githubServiceMoq.Setup(s => s.GetUsers(new List<string> {
                "nonExistingUser01",
                "nonExistingUser02",
                "nonExistingUser03"
            })).ReturnsAsync(new GithubServiceMessage<List<GithubUser>>(new List<GithubUser>(), null));
            return githubServiceMoq.Object;
        }

        public IGithubService GithubService { get; set; }
    }
}
