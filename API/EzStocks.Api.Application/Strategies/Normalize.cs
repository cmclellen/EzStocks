namespace EzStocks.Api.Application.Strategies
{
    public static class Normalize
    {
        public static decimal Transform(decimal price, decimal min, decimal max)
        {
            var decominator = max - min;
            return decominator == 0 ? 1M : (price - min) / decominator;
        }
    }
}
