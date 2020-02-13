using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GithubUsersApi.ModelBinders;
using GithubUsersApi.Models;
using GithubUsersApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GithubUsersApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GithubberController : ControllerBase
    {
        private IGithubService _githubService { get; }

        public GithubberController(
            IGithubService githubService
        )
        {
            _githubService = githubService;
        }

        [HttpGet]
        public async Task<ActionResult<List<GithubUser>>> Get(
            [ModelBinder(BinderType = typeof(UsernamesBinder))]
            List<string> usernames
        )
        {
            var githubUsers = new List<GithubUser>();

            if (usernames == null)
                return githubUsers;

            if (usernames.Count < 1)
            {
                return BadRequest("Number of usernames should be 1 - 10");
            }

            githubUsers = await _githubService.GetUsers(usernames);

            return githubUsers;
        }
    }
}