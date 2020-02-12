using GithubUsersApi.Models;
using GithubUsersApi.Services;
using Moq;
using System.Collections.Generic;

namespace GithubUsersApi.Tests.Fixtures
{
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
                .ReturnsAsync(new List<GithubUser>(users));
            githubServiceMoq.Setup(s => s.GetUsers(new List<string> {
                "randomUser01",
                "randomUser02",
                "randomUser03",
                "nonExistingUser01"
            })).ReturnsAsync(new List<GithubUser>(users));
            githubServiceMoq.Setup(s => s.GetUsers(new List<string> {
                "nonExistingUser01",
                "nonExistingUser02",
                "nonExistingUser03"
            })).ReturnsAsync(new List<GithubUser>());
            return githubServiceMoq.Object;
        }

        public IGithubService GithubService { get; set; }
    }
}
