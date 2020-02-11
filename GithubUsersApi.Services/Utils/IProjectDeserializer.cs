using GithubUsersApi.Models;
using Newtonsoft.Json;

namespace GithubUsersApi.Services.Utils
{
    public interface IProjectDeserializer
    {
        GithubUser Deserialize(JsonTextReader jsonTextReader);
    }
}