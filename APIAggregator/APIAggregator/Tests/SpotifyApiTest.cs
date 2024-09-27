namespace APIAggregator.Tests
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using APIAggregator.Services;
    using Xunit;

    public class SpotifyApiTests
    {
        private readonly SpotifyApi _spotifyApi;

        public SpotifyApiTests()
        {

            _spotifyApi = new SpotifyApi(new HttpClient());
        }

        [Fact]
        public async Task GetSpotifyData_ReturnsData_WhenSongExists()
        {
            // Act
            var result = await _spotifyApi.GetSpotifyData("Some Song");

            // Assert
            Assert.NotNull(result); // Check that result is not null
            Assert.Contains("Some Song", result); // Check that result contains the song name
        }
    }
}
