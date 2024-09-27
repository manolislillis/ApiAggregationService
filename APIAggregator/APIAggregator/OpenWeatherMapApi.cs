using APIAggregator.Intefaces;
using Newtonsoft.Json;

namespace APIAggregator.Services
{
    public class OpenWeatherMapApi : IWeather
    {
        private  readonly HttpClient _httpClient;
        private readonly string _apiKey = "";//Add Your Key

        public OpenWeatherMapApi(HttpClient httpClient) 
        { 
            _httpClient = httpClient;
        }
        
        public async Task<string> GetWeatherInfo(string city)
        {
            try
            {
                var response = await _httpClient.GetAsync("https://api.openweathermap.org/data/2.5/weather?q="+city+"&APPID=" + _apiKey);
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        // Return null if city not found
                        return null;
                    }
                    //for other errors
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Unable to fetch weather data. Status code: {response.StatusCode}. Message: {errorMessage}");
                }
                var weatherData = await response.Content.ReadAsStringAsync();
                return weatherData;
            }
            catch (Exception ex) {
                return $"Error: {ex.Message}";
            }
        }
    }
}
