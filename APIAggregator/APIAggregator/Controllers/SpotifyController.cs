using APIAggregator.Intefaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace APIAggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpotifyController : ControllerBase
    {
        private readonly ISpotify _services;

        public SpotifyController(ISpotify services)
        {
            _services = services;
        }

        [HttpGet("Search Song")]
        public async Task<IActionResult> SearchSpotify(string songName)
        {
            try
            {
                var result = await _services.GetSpotifyData(songName);
                if (result.StartsWith("Error"))
                {
                    return BadRequest(result);
                }

                var resultJson = JsonConvert.DeserializeObject<dynamic>(result);

                var tracks = resultJson.tracks.items;// Find All Songs
                var trackList = new List<object>();

                for (int i = 0; i < Math.Min(5, tracks.Count); i++) // Get 5 tracks
                {
                    var track = tracks[i];
                    var trackInfo = new
                    {
                        Name = (string)track.name,
                        Artist = (string)track.artists[0].name,
                        Duration = ConvertToMinutes((int)track.duration_ms), 
                        Link = (string)track.external_urls.spotify
                    };
                    trackList.Add(trackInfo);
                }

                return Ok(trackList);
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }


        //Convert Ms to Minutes
        private string ConvertToMinutes(int durationMs)
        {
            int minutes = durationMs / 60000;
            int seconds = (durationMs % 60000) / 1000;
            return $"{minutes} min {seconds} sec";
        }
    }
}
