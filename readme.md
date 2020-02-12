Instructions
1. Run dotnet run --project GithubUsersApi/GithubUsersApi.csproj on the terminal
2. Once started, open a browser or postman and send a request to the end point
    localhost:5001/Githubber
3. To request for multiple usernames, query string should look like usernames=user1,user2,user3
4. Url should be similar to localhost:5001/Githubber?usernames=user1&usernames=user2&usernames=user3

Other Info:
1. In-memory caching is used for caching requests