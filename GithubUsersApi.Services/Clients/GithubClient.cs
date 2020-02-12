using GithubUsersApi.Models;
using GithubUsersApi.Services.Utils;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GithubUsersApi.Services.Clients
{
    public class GithubClient : IGithubClient
    {
        private const string GithubApiUrl = "https://api.github.com/users/";

        private readonly HttpClient _httpClient;

        public GithubClient(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<GithubUser> GetUserByLogin(string username)
        {
            var requestUrl = String.Format("{0}{1}", GithubApiUrl, username);

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                requestUrl
            );
            request.Headers.Add("User-Agent", "Githubber");

            var result = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            if (result.IsSuccessStatusCode)
            {
                using (var contentStream = await result.Content.ReadAsStreamAsync())
                {
                    var options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;
                    options.IgnoreNullValues = true;
                    options.PropertyNamingPolicy = new SnakeCaseNamingPolicy();
                    return await JsonSerializer.DeserializeAsync<GithubUser>(contentStream, options);
                }
            }

            return null;
        }
    }
}
