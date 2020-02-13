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
        private const string GithubUserEndpoint = "users/";

        private readonly HttpClient _httpClient;

        public GithubClient(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<GithubUser> GetUserByLogin(string username)
        {
            var requestUrl = String.Format("{0}{1}", GithubUserEndpoint, username);

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                requestUrl
            );

            var result = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            if (result.IsSuccessStatusCode)
            {
                using (var contentStream = await result.Content.ReadAsStreamAsync())
                {
                    var options = new JsonSerializerOptions();
                    options.PropertyNamingPolicy = new SnakeCaseNamingPolicy();
                    return await JsonSerializer.DeserializeAsync<GithubUser>(contentStream, options);
                }
            }

            return null;
        }
    }
}
