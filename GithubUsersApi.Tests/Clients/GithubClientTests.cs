using Xunit;
using System.Net.Http;
using GithubUsersApi.Tests.Helpers;
using System.Web.Http;
using System.Net;
using System.Threading.Tasks;
using GithubUsersApi.Services.Clients;
using GithubUsersApi.Services.Utils;
using Moq;
using Newtonsoft.Json;
using System.IO;
using GithubUsersApi.Models;

namespace GithubUsersApi.Tests.Clients
{
    public class GithubClientTests
    {
        [Fact]
        public async void GetUserByLogin_InputExisting_ReturnGithubUser()
        {
            var expectedContent = new 
            {
                  login= "destiny07",
                  id= 6240204,
                  node_id ="",
                  avatar_url= "https://avatars0.githubusercontent.com/u/6240204?v=4",
                  gravatar_id= "",
                  url= "https://api.github.com/users/destiny07",
                  html_url= "https://github.com/destiny07",
                  followers_url= "https://api.github.com/users/destiny07/followers",
                  following_url= "https://api.github.com/users/destiny07/following{/other_user}",
                  gists_url= "https://api.github.com/users/destiny07/gists{/gist_id}",
                  starred_url= "https://api.github.com/users/destiny07/starred{/owner}{/repo}",
                  subscriptions_url= "https://api.github.com/users/destiny07/subscriptions",
                  organizations_url= "https://api.github.com/users/destiny07/orgs",
                  repos_url= "https://api.github.com/users/destiny07/repos",
                  events_url= "https://api.github.com/users/destiny07/events{/privacy}",
                  received_events_url= "https://api.github.com/users/destiny07/received_events",
                  type= "User",
                  site_admin= false,
                  name= "Destiny Awbelisk",
                  company= "",
                  blog= "",
                  location= "",
                  email= "",
                  hireable= "",
                  bio= "",
                  public_repos= 4,
                  public_gists= 1,
                  followers= 2,
                  following= 5,
                  created_at= "2013-12-22T08:06:40Z",
                  updated_at= "2020-02-10T07:14:32Z"
            };
            var configuration = new HttpConfiguration();
            var clientHandlerStub = new DelegatingHandlerStub((request, cancellationToken) =>
            {
                request.SetConfiguration(configuration);
                var response = request.CreateResponse(
                    HttpStatusCode.OK,
                    expectedContent
                );
                return Task.FromResult(response);
            });
            var client = new HttpClient(clientHandlerStub);
            var projectDeserializerFake = new ProjectDeserializerFake();

            var githubClient = new GithubClient(client, projectDeserializerFake);

            var githubUser = await githubClient.GetUserByLogin("destiny07");

            Assert.NotNull(githubUser);
        }

        [Fact]
        public async void GetUserByLogin_InputNonExisting_ReturnNull()
        {
            var configuration = new HttpConfiguration();
            var clientHandlerStub = new DelegatingHandlerStub((request, cancellationToken) =>
            {
                request.SetConfiguration(configuration);
                var response = request.CreateResponse(
                    HttpStatusCode.NotFound,
                    ""
                );
                return Task.FromResult(response);
            });
            var client = new HttpClient(clientHandlerStub);
            var projectDeserializerMoq = new Mock<IProjectDeserializer>();

            var githubClient = new GithubClient(client, projectDeserializerMoq.Object);

            var githubUser = await githubClient.GetUserByLogin("asdafdsf");

            Assert.Null(githubUser);
        }
    }
}