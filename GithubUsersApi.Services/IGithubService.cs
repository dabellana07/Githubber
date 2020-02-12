using System.Collections.Generic;
using System.Threading.Tasks;
using GithubUsersApi.Models;

namespace GithubUsersApi.Services
{
    public interface IGithubService
    {
        Task<List<GithubUser>> GetUsers(List<string> usernames);
    }
}