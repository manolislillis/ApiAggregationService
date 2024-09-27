namespace APIAggregator.Intefaces
{
    public interface ISpotify
    {
        Task<string> GetSpotifyData(string songName);
    }
}
