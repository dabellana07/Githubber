using GithubUsersApi.Models;
using GithubUsersApi.Services.Utils;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GithubUsersApi.Services.Clients
{
    public class GithubClient : IGithubClient
    {
        private const string GithubApiUrl = "https://api.github.com/users/";

        private readonly HttpClient _httpClient;
        private readonly IProjectDeserializer _projectDeserializer;

        public GithubClient(
            HttpClient httpClient,
            IProjectDeserializer projectDeserializer
        )
        {
            this._httpClient = httpClient;
            this._projectDeserializer = projectDeserializer;
        }

        public async Task<GithubUser> GetUserByLogin(string username)
        {
            try
            {
                var requestUrl = String.Format("{0}{1}", GithubApiUrl, username);

                var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    requestUrl
                );
                request.Headers.Add("User-Agent", "Githubber");

                var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    using (var streamReader = new StreamReader(await response.Content.ReadAsStreamAsync()))
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                    {
                        return _projectDeserializer.Deserialize(jsonTextReader);
                    }
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
