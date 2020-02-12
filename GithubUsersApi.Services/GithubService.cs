using GithubUsersApi.Messages;
using GithubUsersApi.Models;
using GithubUsersApi.Services.Clients;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GithubUsersApi.Services
{
    public class GithubService : IGithubService
    {
        private readonly ICacheService _cacheService;
        private readonly IGithubClient _githubClient;

        public GithubService(
            ICacheService cacheService,
            IGithubClient githubClient
        )
        {
            _cacheService = cacheService;
            _githubClient = githubClient;
        }

        public async Task<GithubServiceMessage<List<GithubUser>>> GetUsers(List<string> usernames)
        {
            try 
            {
                var usernamesToProcess = usernames.Count <= 10
                    ? usernames
                    : usernames.GetRange(0, 10);
                var githubUsers = new List<GithubUser>();

                foreach (var username in usernamesToProcess)
                {
                    GithubUser userFromCache = _cacheService.GetGithubUser(username);

                    if (userFromCache == null)
                    {
                        var userFromApi = await _githubClient.GetUserByLogin(username);

                        if (userFromApi != null)
                        {
                            _cacheService.SetGithubUser(username, userFromApi);
                            githubUsers.Add(userFromApi);
                        }
                    }
                    else
                    {
                        githubUsers.Add(userFromCache);
                    }
                }

                return new GithubServiceMessage<List<GithubUser>>(githubUsers, null);
            }
            catch (Exception ex)
            {
                return new GithubServiceMessage<List<GithubUser>>(null, ex);
            }
        }
    }
}