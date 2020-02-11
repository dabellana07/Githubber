using GithubUsersApi.Messages;
using GithubUsersApi.Models;
using GithubUsersApi.Services.Clients;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GithubUsersApi.Services
{
    public class GithubService : IGithubService
    {
        private readonly IGithubClient _githubClient;

        public GithubService(IGithubClient githubClient)
        {
            this._githubClient = githubClient;
        }

        public async Task<GithubServiceMessage<GithubUser>> GetUser(string username)
        {
            try 
            {
                GithubUser user = await _githubClient.GetUserByLogin(username);

                return new GithubServiceMessage<GithubUser>(user, null);
            }
            catch (Exception ex)
            {
                return new GithubServiceMessage<GithubUser>(null, ex);
            }
        }
    }
}