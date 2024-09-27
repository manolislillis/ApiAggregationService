using APIAggregator.Intefaces;

namespace APIAggregator.Services
{
    public class NewApi : INews
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "";//Add Your Key

        public NewApi(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _httpClient.DefaultRequestHeaders.Add("User-Agent", "APIAggregator/1.0");
        }

        public async Task<string> GetNewsData(string topic, int c)
        {
            try
            {//url Request 
                var response = await _httpClient.GetAsync($"https://newsapi.org/v2/everything?q={topic}&apiKey=" + _apiKey + $"&pageSize={c}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return $"Error: Unable to fetch news data. Status code: {response.StatusCode}. Message: {errorMessage}";
                }

                var newsData = await response.Content.ReadAsStringAsync();
                return newsData;

            }
            catch (Exception ex) { 
                return $"Error: An unexpected error occurred. Message: {ex.Message}";
            }
        }
    }
}
