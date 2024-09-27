using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using APIAggregator.Intefaces;

namespace APIAggregator.Services
{
    public class SpotifyApi : ISpotify
    {
        private readonly HttpClient _httpClient;
        private readonly string _clientId = "";//Add Your Key
        private readonly string _clientSecret = "";//Add Yopur Key
        private string _accessToken;


        public SpotifyApi (HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public async Task Authanticate()
        {
            var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            var requestBody = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" }
            };

            var response = await _httpClient.PostAsync("https://accounts.spotify.com/api/token", new FormUrlEncodedContent(requestBody));
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonDocument.Parse(responseBody);
                _accessToken = jsonResponse.RootElement.GetProperty("access_token").GetString();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            }
            else
            {
                throw new Exception("Unable to authenticate with Spotify API.");
            }
        }

        public async Task<string> GetSpotifyData(string songName)
        {
            await Authanticate();// Check Valid Token 


            var encodedSongName = Uri.EscapeDataString(songName);// Encode the name of the song For spotify Url
            
            var response = await _httpClient.GetAsync($"https://api.spotify.com/v1/search?q={encodedSongName}&type=track");
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) {
                throw new Exception($"Error fetching song details: {response.StatusCode} . Messaga: {responseBody}");
            }

            var songData = await response.Content.ReadAsStringAsync();
            return songData;
        }
    }
}
