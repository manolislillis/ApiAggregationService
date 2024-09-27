using APIAggregator.Intefaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace APIAggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AggregationController : ControllerBase
    {
        
        [HttpGet("aggregate")]
        public async Task<IActionResult> GetAggregatedData(string city, string topic, string songName, int articleCount = 3)
        {
            try
            {
                // Request Data 
                var weatherService = HttpContext.RequestServices.GetService<IWeather>();
                var spotifyService = HttpContext.RequestServices.GetService<ISpotify>();
                var newsService = HttpContext.RequestServices.GetService<INews>();

                // Check for Failure
                if (weatherService == null)
                    return StatusCode(500, "Failed to resolve Weather service.");
                if (spotifyService == null)
                    return StatusCode(500, "Failed to resolve Spotify service.");
                if (newsService == null)
                    return StatusCode(500, "Failed to resolve News service.");

                // Call data and convert to Json
                //Weather
                var weatherData = await weatherService.GetWeatherInfo(city);
                var weatherDataJson = JsonConvert.DeserializeObject<dynamic>(weatherData);
                //Spotify
                var spotifyData = await spotifyService.GetSpotifyData(songName);
                var spotifyDataJson = JsonConvert.DeserializeObject<dynamic>(spotifyData);
                var spotifyTracks = spotifyDataJson.tracks.items;
                var spotifyTrackList = new List<object>();

                for (int i = 0; i < Math.Min(5, spotifyTracks.Count); i++) // Get 5 tracks
                {
                    var track = spotifyTracks[i];
                    var trackInfo = new
                    {
                        Name = (string)track.name,
                        Artist = (string)track.artists[0].name,
                        Duration = ConvertToMinutes((int)track.duration_ms),
                        Link = (string)track.external_urls.spotify
                    };
                    spotifyTrackList.Add(trackInfo);
                }

                //News
                var newsData = await newsService.GetNewsData(topic, articleCount);
                var newsDataJson = JsonConvert.DeserializeObject<dynamic>(newsData);
                JArray articles = newsDataJson.articles as JArray;

                var newsResults = new
                {
                    Topic = topic,
                    Articles = articles.Select(article => new
                    {
                        Author = (string)article["author"] ?? "Unknown Author", // Default value if author is null
                        Title = (string)article["title"],
                        Url = (string)article["url"]
                    }).ToList()
                };

                // Combine the results into one response
                var aggregatedData = new
                {
                    Weather = new
                    {
                        City = (string)weatherDataJson.name,
                        Country = (string)weatherDataJson.sys.country,
                        Temperature = (float)weatherDataJson.main.temp,
                        WindSpeed = (float)weatherDataJson.wind.speed
                    },
                    Blank1 = "-----------------------------------------",
                    Spotify = spotifyTrackList,
                    Blank2 = "-----------------------------------------",
                    News = newsResults
                };

                return Ok(aggregatedData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        private string ConvertToMinutes(int durationMs)
        {
            int minutes = durationMs / 60000;
            int seconds = (durationMs % 60000) / 1000;
            return $"{minutes} min {seconds} sec";
        }
    }

}
