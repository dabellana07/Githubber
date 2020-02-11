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
    public class GithubberControllerTests : IClassFixture<GithubServiceFixture>
    {
        private GithubServiceFixture _githubServiceFixture { get; set; }

        public GithubberControllerTests(GithubServiceFixture githubServiceFixture)
        {
            _githubServiceFixture = githubServiceFixture;
        }

        [Fact]
        public async void Get_InputExisting_ReturnList()
        {
            var memoryCacheFake = new PassiveMemoryCacheFake();

            var controller = new GithubberController(
                memoryCacheFake,
                _githubServiceFixture.GithubService
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
            var memoryCacheFake = new PassiveMemoryCacheFake();

            var controller = new GithubberController(
                memoryCacheFake,
                _githubServiceFixture.GithubService
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
            var memoryCacheFake = new PassiveMemoryCacheFake();

            var controller = new GithubberController(
                memoryCacheFake,
                _githubServiceFixture.GithubService
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
            var memoryCacheFake = new PassiveMemoryCacheFake();
            var githubServiceMoq = new Mock<IGithubService>();

            var controller = new GithubberController(
                memoryCacheFake,
                _githubServiceFixture.GithubService
            );

            var result = await controller.Get(new List<string>());

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async void Get_InputIsMoreThanTen_ReturnBadRequest()
        {
            var memoryCacheFake = new PassiveMemoryCacheFake();

            var controller = new GithubberController(
                memoryCacheFake,
                _githubServiceFixture.GithubService
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
    }

    public class GithubServiceFixture
    {
        public GithubServiceFixture()
        {
            GithubService = InitGithubService();
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

        public IGithubService GithubService { get; set; }
    }
}
