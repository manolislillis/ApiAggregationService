namespace APIAggregator.Tests
{
    using APIAggregator.Services;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Xunit;

    public class OpenWeatherMapApiTests
    {
        private readonly OpenWeatherMapApi _weatherApi;

        public OpenWeatherMapApiTests()
        {
            // Initialize your API class
            _weatherApi = new OpenWeatherMapApi(new HttpClient());
        }

        [Fact]
        public async Task GetWeatherData_ReturnsData_WhenCityExists()
        {
            // Act
            var result = await _weatherApi.GetWeatherInfo("London");

            // Assert
            Assert.NotNull(result); // Check that result is not null
            Assert.Contains("London", result); // Check that result contains the city name
        }
    }
}
