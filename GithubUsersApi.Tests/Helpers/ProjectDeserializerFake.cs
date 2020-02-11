using GithubUsersApi.Models;
using GithubUsersApi.Services.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GithubUsersApi.Tests.Helpers
{
    public class ProjectDeserializerFake : IProjectDeserializer
    {
        public GithubUser Deserialize(JsonTextReader jsonTextReader)
        {
            return new GithubUser
            {
                Login = "destiny07",
                Name = "Destiny Awbelisk",
                Followers = 2,
                PublicRepos = 4
            };
        }
    }
}
