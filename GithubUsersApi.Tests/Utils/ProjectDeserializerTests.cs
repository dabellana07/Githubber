﻿using GithubUsersApi.Services.Utils;
using Newtonsoft.Json;
using System.IO;
using Xunit;

namespace GithubUsersApi.Tests.Utils
{
    public class ProjectDeserializerTests
    {
        [Fact]
        public void Deserialize_MatchingFields_ShouldReturnGithubUser()
        {
            var json = @"{
                ""login"" : ""destiny07"",
                ""id"" : 6240204,
                ""node_id"" : """",
                ""avatar_url"" : ""https://avatars0.githubusercontent.com/u/6240204?v=4"",
                ""gravatar_id"" : """",
                ""url"" : ""https://api.github.com/users/destiny07"",
                ""html_url"" : ""https://github.com/destiny07"",
                ""followers_url"" : ""https://api.github.com/users/destiny07/followers"",
                ""following_url"" : ""https://api.github.com/users/destiny07/following{/other_user}"",
                ""gists_url"" : ""https://api.github.com/users/destiny07/gists{/gist_id}"",
                ""starred_url"" : ""https://api.github.com/users/destiny07/starred{/owner}{/repo}"",
                ""subscriptions_url"" : ""https://api.github.com/users/destiny07/subscriptions"",
                ""organizations_url"" : ""https://api.github.com/users/destiny07/orgs"",
                ""repos_url"" : ""https://api.github.com/users/destiny07/repos"",
                ""events_url"" : ""https://api.github.com/users/destiny07/events{/privacy}"",
                ""received_events_url"" : ""https://api.github.com/users/destiny07/received_events"",
                ""type"" : ""User"",
                ""site_admin"" : false,
                ""name"" : ""Destiny Awbelisk"",
                ""company"" : """",
                ""blog"" : """",
                ""location"" : """",
                ""email"" : """",
                ""hireable"" : """",
                ""bio"" : """",
                ""public_repos"" : 4,
                ""public_gists"" : 1,
                ""followers"" : 2,
                ""following"" : 5,
                ""created_at"" : ""2013-12-22T08:06:40Z"",
                ""updated_at"" : ""2020-02-10T07:14:32Z""
            }";

            var jsonTextReader = new JsonTextReader(new StringReader(json));

            var projectDeserializer = new ProjectDeserializer();

            var githubUser = projectDeserializer.Deserialize(jsonTextReader);

            Assert.Equal("Destiny Awbelisk", githubUser.Name);
        }

        [Fact]
        public void Deserialize_UnmatchingFields_ShouldReturnNull()
        {
            var json = @"{
                ""number"" : 1,
                ""letter"" : ""a"",
                ""note"" : ""do"",
            }";
            var jsonTextReader = new JsonTextReader(new StringReader(json));

            var projectDeserializer = new ProjectDeserializer();

            var githubUser = projectDeserializer.Deserialize(jsonTextReader);

            Assert.Null(githubUser.Name);
        }
    }
}
