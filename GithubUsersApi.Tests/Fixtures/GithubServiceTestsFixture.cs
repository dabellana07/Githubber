﻿using GithubUsersApi.Models;
using GithubUsersApi.Services;
using GithubUsersApi.Services.Clients;
using Moq;
using System.Collections.Generic;

namespace GithubUsersApi.Tests.Fixtures
{
    public class GithubServiceTestsFixture
    {
        public GithubServiceTestsFixture()
        {
            CacheService = InitCacheService();
        }

        public void InitGithubClient(List<GithubUser> githubUsers = null)
        {
            var githubClientMoq = new Mock<IGithubClient>();

            if (githubUsers != null)
            {
                foreach (var githubUser in githubUsers)
                {
                    githubClientMoq.Setup(s => s.GetUserByLogin(githubUser.Login))
                        .ReturnsAsync(githubUser);
                }
            }
            else
            {
                githubClientMoq.Setup(s => s.GetUserByLogin("user01"))
                    .ReturnsAsync(new GithubUser
                    {
                        Name = "User One",
                        Login = "user01",
                        Company = null,
                        PublicRepos = 3,
                        Followers = 10
                    });
                githubClientMoq.Setup(s => s.GetUserByLogin("user05"))
                    .ReturnsAsync(new GithubUser
                    {
                        Name = "User Five",
                        Login = "user05",
                        Company = null,
                        PublicRepos = 5,
                        Followers = 3
                    });
                githubClientMoq.Setup(s => s.GetUserByLogin("user06"))
                    .ReturnsAsync(new GithubUser
                    {
                        Name = "User Six",
                        Login = "user06",
                        Company = null,
                        PublicRepos = 3,
                        Followers = 10
                    });
                githubClientMoq.Setup(s => s.GetUserByLogin("user07"))
                    .ReturnsAsync(new GithubUser
                    {
                        Name = "User Seven",
                        Login = "user07",
                        Company = null,
                        PublicRepos = 3,
                        Followers = 10
                    });
                githubClientMoq.Setup(s => s.GetUserByLogin("user08"))
                    .ReturnsAsync(new GithubUser
                    {
                        Name = "User Eight",
                        Login = "user08",
                        Company = null,
                        PublicRepos = 3,
                        Followers = 10
                    });
                githubClientMoq.Setup(s => s.GetUserByLogin("user09"))
                    .ReturnsAsync(new GithubUser
                    {
                        Name = "User Nine",
                        Login = "user08",
                        Company = null,
                        PublicRepos = 3,
                        Followers = 10
                    });
                githubClientMoq.Setup(s => s.GetUserByLogin("user10"))
                    .ReturnsAsync(new GithubUser
                    {
                        Name = "User Ten",
                        Login = "user10",
                        Company = null,
                        PublicRepos = 3,
                        Followers = 10
                    });
                githubClientMoq.Setup(s => s.GetUserByLogin("user11"))
                    .ReturnsAsync(new GithubUser
                    {
                        Name = "User Eleven",
                        Login = "user11",
                        Company = null,
                        PublicRepos = 3,
                        Followers = 10
                    });
                githubClientMoq.Setup(s => s.GetUserByLogin("user12"))
                    .ReturnsAsync(new GithubUser
                    {
                        Name = "User Twelve",
                        Login = "user12",
                        Company = null,
                        PublicRepos = 3,
                        Followers = 10
                    });
                githubClientMoq.Setup(s => s.GetUserByLogin("user13"))
                    .ReturnsAsync(new GithubUser
                    {
                        Name = "User Thirteen",
                        Login = "user13",
                        Company = null,
                        PublicRepos = 3,
                        Followers = 10
                    });
                githubClientMoq.Setup(s => s.GetUserByLogin("user14"))
                    .ReturnsAsync(new GithubUser
                    {
                        Name = "User Fourteen",
                        Login = "user14",
                        Company = null,
                        PublicRepos = 3,
                        Followers = 10
                    });
                githubClientMoq.Setup(s => s.GetUserByLogin("user15"))
                    .ReturnsAsync(new GithubUser
                    {
                        Name = "User Fifteen",
                        Login = "user15",
                        Company = null,
                        PublicRepos = 3,
                        Followers = 10
                    });
                githubClientMoq.Setup(s => s.GetUserByLogin("user02"))
                    .ReturnsAsync((GithubUser)null);
            }
            GithubClient = githubClientMoq.Object;
        }

        private ICacheService InitCacheService()
        {
            var cacheServiceMoq = new Mock<ICacheService>();
            cacheServiceMoq
                .Setup(s => s.GetGithubUser("fromCache01"))
                .Returns(new GithubUser
                {
                    Name = "From CacheOne",
                    Login = "fromCache01",
                    Company = null,
                    PublicRepos = 1,
                    Followers = 1
                });
            
            return cacheServiceMoq.Object;
        }

        public IGithubClient GithubClient { get; set; }

        public ICacheService CacheService { get; set; }
    }
}
