using EzStocks.Api.Application.Services;

namespace EzStocks.Api.Infrastructure.Alphavantage.Builders
{
    public class TickerSymbolBuilder
    {
        private string? _symbol, _name, _region, _timeZone, _currency;

        public TickerSymbolBuilder SetSymbol(string symbol)
        {
            _symbol = symbol;
            return this;
        }

        public TickerSymbolBuilder SetName(string name)
        {
            _name = name;
            return this;
        }

        public TickerSymbolBuilder SetRegion(string region)
        {
            _region = region;
            return this;
        }

        public TickerSymbolBuilder SetTimeZone(string timeZone)
        {
            _timeZone = timeZone;
            return this;
        }

        public TickerSymbolBuilder SetCurrency(string currency)
        {
            _currency = currency;
            return this;
        }

        public TickerSymbol Build()
        {
            return new TickerSymbol(_symbol!, _name!, _region!, _currency!);
        }
    }
}
