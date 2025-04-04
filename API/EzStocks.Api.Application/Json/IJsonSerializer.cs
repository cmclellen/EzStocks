﻿namespace EzStocks.Api.Application.Json
{
    public interface IJsonSerializer
    {
        T? Deserialize<T>(string json);
        string Serialize<T>(T t);
    }
}
