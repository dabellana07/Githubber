using GithubUsersApi.Models;
using GithubUsersApi.Services;
using GithubUsersApi.Services.Clients;
using Moq;
using Xunit;

namespace GithubUsersApi.Tests.Services
{
    public class GithubServiceTests : IClassFixture<GithubClientFixture>
    {
        private readonly GithubClientFixture _githubClientFixture;

        public GithubServiceTests(GithubClientFixture githubClientFixture)
        {
            this._githubClientFixture = githubClientFixture;
        }

        [Fact]
        public async void GetUser_GithubUserExisting_ReturnWithNonNullUser()
        {
            var githubService = new GithubService(_githubClientFixture.GithubClient);

            var githubServiceMessage = await githubService.GetUser("user01");

            Assert.NotNull(githubServiceMessage.Message);
        }

        [Fact]
        public async void GetUser_GithubUserNonExisting_ReturnWithNullUser()
        {
            var githubService = new GithubService(_githubClientFixture.GithubClient);

            var githubServiceMessage = await githubService.GetUser("user02");

            Assert.Null(githubServiceMessage.Message);
        }
    }

    public class GithubClientFixture
    {
        public GithubClientFixture()
        {
            GithubClient = InitGithubClient();
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

        public IGithubClient GithubClient { get; set; }
    }
}
