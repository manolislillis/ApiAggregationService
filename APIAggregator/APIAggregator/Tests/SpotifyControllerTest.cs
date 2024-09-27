namespace APIAggregator.Tests
{
    using APIAggregator.Controllers;
    using APIAggregator.Services;
    using Microsoft.AspNetCore.Mvc;
    using Xunit;

    public class SpotifyControllerTests
    {
        private readonly SpotifyController _controller;

        public SpotifyControllerTests()
        {

            _controller = new SpotifyController(new SpotifyApi(new HttpClient()));
        }

        [Fact]
        public async Task SearchSpotify_ReturnsOkResult_WhenSongFound()
        {
            // Act
            var result = await _controller.SearchSpotify("Some Song");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value); // Check that value is not null
        }
    }
}


