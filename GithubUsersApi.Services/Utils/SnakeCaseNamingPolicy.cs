using GithubUsersApi.Services.Extensions;
using System.Text.Json;

namespace GithubUsersApi.Services.Utils
{
    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return name.ToSnakeCase();
        }
    }
}
