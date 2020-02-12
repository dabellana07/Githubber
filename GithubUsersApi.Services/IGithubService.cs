using System.Collections.Generic;
using System.Threading.Tasks;
using GithubUsersApi.Messages;
using GithubUsersApi.Models;

namespace GithubUsersApi.Services
{
    public interface IGithubService
    {
        Task<GithubServiceMessage<List<GithubUser>>> GetUsers(List<string> usernames);
    }
}