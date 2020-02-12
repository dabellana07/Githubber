using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GithubUsersApi.ModelBinders;
using GithubUsersApi.Models;
using GithubUsersApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace GithubUsersApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GithubberController : ControllerBase
    {
        private ICacheService _cacheService;
        private IGithubService _githubService { get; }

        public GithubberController(
            ICacheService cacheService,
            IGithubService githubService
        )
        {
            _cacheService = cacheService;
            _githubService = githubService;
        }

        [HttpGet]
        public async Task<ActionResult<List<GithubUser>>> Get(
            [ModelBinder(BinderType = typeof(UsernamesBinder))]
            List<string> usernames
        )
        {
            try
            {
                if (usernames.Count < 1 || usernames.Count > 10)
                {
                    return BadRequest("Number of usernames should be 1 - 10");
                }

                var githubUsers = new List<GithubUser>();
                foreach (var githubUsername in usernames)
                {
                    GithubUser userFromCache = _cacheService.GetGithubUser(githubUsername);

                    if (userFromCache == null)
                    {
                        var response = await _githubService.GetUser(githubUsername);
                        if (!response.HasException)
                        {
                            var githubUser = response.Message;

                            _cacheService.SetGithubUser(githubUsername, githubUser);
                            githubUsers.Add(githubUser);
                        }
                    }
                    else
                    {
                        githubUsers.Add(userFromCache);
                    }
                }

                return githubUsers.OrderBy(g => g.Name).ToList();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}