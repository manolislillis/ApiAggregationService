namespace APIAggregator.Tests
{
    using APIAggregator.Services;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Xunit;

    public class NewsApiTests
    {
        private readonly NewApi _newsApi;

        public NewsApiTests()
        {
            _newsApi = new NewApi(new HttpClient());
        }

        [Fact]
        public async Task GetNewsData_ReturnsData_WhenTopicExists()
        {
            // Act
            var result = await _newsApi.GetNewsData("Technology", 5); // Example topic and count

            // Assert
            Assert.NotNull(result); // Check that result is not null
            Assert.Contains("Technology", result); // Check that result contains the topic name 
        }
    }
}
