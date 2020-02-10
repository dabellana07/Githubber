using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using GithubUsersApi.Models;

namespace GithubUsersApi.Services
{
    public class GithubService : IGithubService
    {
        private const string GithubApiUrl = "https://api.github.com/users/";
        
        private IHttpClientFactory _clientFactory;

        public GithubService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<GithubServiceMessage<GithubUser>> GetUser(string username)
        {
            try 
            {
                var requestUrl = GithubApiUrl + username;

                var client = _clientFactory.CreateClient();

                var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    requestUrl
                );
                request.Headers.Add("Accept", "application/vnd.github.v3+json");
                request.Headers.Add("User-Agent", "Githubber");

                var response = await client.SendAsync(request);

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