using APIAggregator.Intefaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using Newtonsoft.Json;

namespace APIAggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpenWeatherMapApiController : ControllerBase
    {
        private readonly IWeather _services;

        public OpenWeatherMapApiController(IWeather services)
        {
            _services = services;
        }


        [HttpGet("weather")]
        public async Task<IActionResult> GetWeatherInfo(string city)
        {
            try
            {
                var result = await _services.GetWeatherInfo(city);

                if (result == null)
                {
                    //Error 404
                    return NotFound(new { Message = "Weather Data Not Found. Maybe Wrong City Name" });
                }

                var resultJson = JsonConvert.DeserializeObject<dynamic>(result);

                var outputData = new
                {
                    City = (string)resultJson.name,
                    Country = (string)resultJson.sys.country,
                    Temperature = (float)resultJson.main.temp,
                    Wind = (float)resultJson.wind.speed,

                };
                return Ok(outputData);
            }
            catch (Exception ex) {

                //Handle error 500  
                return StatusCode(500, new { Message = "Eroor Fetching the Data" , Details = ex.Message });
            }
            
        }
    }
}
