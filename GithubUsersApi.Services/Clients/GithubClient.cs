using GithubUsersApi.Models;
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
            try
            {
                var requestUrl = GithubApiUrl + username;

                var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    requestUrl
                );
                request.Headers.Add("Accept", "application/vnd.github.v3+json");
                request.Headers.Add("User-Agent", "Githubber");

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    var jsonSerializerOptions = new JsonSerializerOptions();
                    jsonSerializerOptions.PropertyNameCaseInsensitive = true;

                    var githubUser = JsonSerializer.Deserialize<GithubUser>(responseString, jsonSerializerOptions);

                    return githubUser;
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
