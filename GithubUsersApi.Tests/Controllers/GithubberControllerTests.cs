using GithubUsersApi.Controllers;
using GithubUsersApi.Services;
using GithubUsersApi.Tests.Fixtures;
using Microsoft.AspNetCore.Mvc;
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

            var result = await controller.Get(null);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
    }
}
