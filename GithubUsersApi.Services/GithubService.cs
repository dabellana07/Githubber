using GithubUsersApi.Messages;
using GithubUsersApi.Models;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GithubUsersApi.Services
{
    public class GithubService : IGithubService
    {
        private const string GithubApiUrl = "https://api.github.com/users/";

        private readonly HttpClient _httpClient;

        public GithubService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GithubServiceMessage<GithubUser>> GetUser(string username)
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
                    return new GithubServiceMessage<GithubUser>(githubUser, null);
                }
                else
                {
                    return new GithubServiceMessage<GithubUser>(null, new Exception($"Exception occurred: {response.ReasonPhrase}"));
                }
            }
            catch (Exception ex)
            {
                return new GithubServiceMessage<GithubUser>(null, ex);
            }
        }
    }
}