using System.Threading.Tasks;
using GithubUsersApi.Models;

namespace GithubUsersApi.Services
{
    public interface IGithubService
    {
        Task<GithubServiceMessage<GithubUser>> GetUser(string username);
    }
}