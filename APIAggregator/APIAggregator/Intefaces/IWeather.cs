namespace APIAggregator.Intefaces
{
    public interface IWeather
    {
        Task<string> GetWeatherInfo(string city);
    }
}
