using GithubUsersApi.Models;
using System.Threading.Tasks;

namespace GithubUsersApi.Services.Clients
{
    public interface IGithubClient
    {
        Task<GithubUser> GetUserByLogin(string username);
    }
}