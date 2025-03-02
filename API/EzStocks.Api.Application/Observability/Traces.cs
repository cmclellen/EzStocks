using System.Diagnostics;

namespace EzStocks.Api.Application.Observability
{
    public static class Traces
    {
        public static readonly ActivitySource DefaultSource = new ActivitySource("ExStock.Api.Default", "1.0.0");
    }
}
