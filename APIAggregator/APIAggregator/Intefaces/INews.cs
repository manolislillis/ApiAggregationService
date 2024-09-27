namespace APIAggregator.Intefaces
{
    public interface INews
    {
        Task<string> GetNewsData(string topic , int articleAmnt);
    }
}
