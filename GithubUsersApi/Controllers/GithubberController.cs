using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly int CacheExpirationMinutes = 2;

        private IMemoryCache _memoryCache;
        private IGithubService _githubService { get; }

        public GithubberController(
            IMemoryCache memoryCache,
            IGithubService githubService
        )
        {
            _memoryCache = memoryCache;
            _githubService = githubService;
        }

        public async Task<ActionResult<List<GithubUser>>> Get([FromQuery(Name = "usernames")] List<string> usernames)
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
                    GithubUser user;

                    if (!_memoryCache.TryGetValue<GithubUser>(githubUsername, out user))
                    {
                        var response = await _githubService.GetUser(githubUsername);
                        if (!response.HasException)
                        {
                            user = response.Message;

                            MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions();
                            cacheExpirationOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(CacheExpirationMinutes);
                            cacheExpirationOptions.Priority = CacheItemPriority.Normal;

                            _memoryCache.Set(githubUsername, user, cacheExpirationOptions);
                            githubUsers.Add(user);
                        }
                    }
                    else
                    {
                        githubUsers.Add(user);
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