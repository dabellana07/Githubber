using GithubUsersApi.Models;
using Newtonsoft.Json;

namespace GithubUsersApi.Services.Utils
{
    public class ProjectDeserializer : IProjectDeserializer
    {
        public GithubUser Deserialize(JsonTextReader jsonTextReader)
        {
            GithubUser githubUser = null;
            var currentPropertyName = string.Empty;

            while (jsonTextReader.Read())
            {
                switch (jsonTextReader.TokenType)
                {
                    case JsonToken.StartObject:
                        githubUser = new GithubUser();
                        continue;
                    case JsonToken.PropertyName:
                        currentPropertyName = jsonTextReader.Value.ToString();
                        continue;
                    case JsonToken.String:
                        switch (currentPropertyName)
                        {
                            case "login":
                                githubUser.Login = jsonTextReader.Value.ToString();
                                continue;
                            case "name":
                                githubUser.Name = jsonTextReader.Value.ToString();
                                continue;
                            case "company":
                                githubUser.Company = jsonTextReader.Value.ToString();
                                continue;

                        }
                        continue;
                    case JsonToken.Integer:
                        switch (currentPropertyName)
                        {
                            case "followers":
                                githubUser.Followers = int.Parse(jsonTextReader.Value.ToString());
                                continue;
                            case "public_repos":
                                githubUser.PublicRepos = int.Parse(jsonTextReader.Value.ToString());
                                continue;
                        }
                        continue;
                }
            }

            return githubUser;
        }
    }
}
