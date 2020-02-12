using GithubUsersApi.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace GithubUsersApi.Tests.Helpers
{
    public class PassiveMemoryCacheFake : IMemoryCache
    {
        private readonly Dictionary<string, GithubUser> _githubUsers = new Dictionary<string, GithubUser>()
        {
            {
                "fromCache01",
                new GithubUser {
                    Name = "From CacheOne",
                    Login = "fromCache01", 
                    Followers = 1, PublicRepos = 1 
                }
            },
            {
                "fromCache02",
                new GithubUser {
                    Name = "From CacheTwo",
                    Login = "fromCache02",
                    Followers = 2, PublicRepos = 2
                }
            },
            {
                "fromCache03",
                new GithubUser {
                    Name = "From CacheThree",
                    Login = "fromCache03",
                    Followers = 3, PublicRepos = 3
                }
            }
        };

        public ICacheEntry CreateEntry(object key)
        {
            return new CacheEntryFake { Key = key };
        }

        public void Dispose() { }

        public void Remove(object key) { }

        public bool TryGetValue(object key, out object value)
        {
            var stringKey = Convert.ToString(key);
            if (_githubUsers.ContainsKey(stringKey))
            {
                value = _githubUsers[stringKey];
                return true;
            }

            value = null;
            return false;
        }
    }
}
