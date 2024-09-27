namespace APIAggregator.Tests
{
    using APIAggregator.Controllers;
    using APIAggregator.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Xunit;

    public class WeatherControllerTests
    {
        private readonly OpenWeatherMapApiController _controller;

        public WeatherControllerTests()
        {

            _controller = new OpenWeatherMapApiController(new OpenWeatherMapApi(new HttpClient())); 
        }

        [Fact]
        public async Task GetWeather_ReturnsOkResult_WhenWeatherFound()
        {
            // Act
            var result = await _controller.GetWeatherInfo("London");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value); // Check that value is not null
        }
    }
}