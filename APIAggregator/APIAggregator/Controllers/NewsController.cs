using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APIAggregator.Services;
using APIAggregator.Intefaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace APIAggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INews _news;

        public NewsController (INews news)
        {
            _news = news;
        }

        [HttpGet("Articles")]
        public async Task<IActionResult> GetNewsData(string topic, int Articles_Amount )
        {
            try
            {


                var result = await _news.GetNewsData(topic, Articles_Amount);
                //Convert results to Json

                if (result.StartsWith("Error"))
                {
                    return BadRequest(result);
                }
                var resultJson = JsonConvert.DeserializeObject<dynamic>(result);
                // Extract Articles in JArray 
                JArray articles = resultJson.articles as JArray;

                var outputData = new
                {
                    Topic = topic,
                    Articles = articles.Select(article => new
                    {
                        Author = (string)article["author"] ?? "Unknown Author", // Default value if author is null
                        Title = (string)article["title"],
                        Url = (string)article["url"]
                    }).ToList()
                };
                return Ok(outputData);
            }
            catch (Exception ex) {
                return StatusCode(500, $"Error: An unexpected error occurred. Message: {ex.Message}");
            }
        }
    }
}
