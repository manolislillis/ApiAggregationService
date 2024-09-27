namespace APIAggregator.Tests
{
    using APIAggregator.Controllers;
    using APIAggregator.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Xunit;

    public class NewsControllerTests
    {
        private readonly NewsController _controller;

        public NewsControllerTests()
        {
            _controller = new NewsController(new NewApi(new HttpClient())); 
        }

        [Fact]
        public async Task GetNews_ReturnsOkResult_WhenNewsFound()
        {
            // Act
            var result = await _controller.GetNewsData("Technology" , 1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value); // Check that value is not null

        }
    }
}
