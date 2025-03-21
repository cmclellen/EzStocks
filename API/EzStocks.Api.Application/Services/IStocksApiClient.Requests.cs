﻿namespace EzStocks.Api.Application.Services
{
    public record GetStockPriceRequest(string Symbol);

    public record SearchForSymbolRequest(string SearchText);

    public record GetStockTickersRequest(string? Ticker = null, int Limit = 100, string? Cursor = null);
}
