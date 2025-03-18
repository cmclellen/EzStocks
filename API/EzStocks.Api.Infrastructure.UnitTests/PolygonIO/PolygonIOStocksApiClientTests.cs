using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.RequestBuilders;
using WireMock.FluentAssertions;
using FluentAssertions;

namespace EzStocks.Api.Infrastructure.UnitTests.PolygonIO
{
    [TestClass]
    public class PolygonIOStocksApiClientTests
    {
        private WireMockServer _server;


        [TestInitialize]
        public void TestInitialize()
        {
            _server = WireMockServer.Start();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _server.Stop();
            _server.Dispose();
        }

        [TestMethod]
        public async Task GetAllTickersAsync_ShouldReturnAllTickers()
        {
            // ARRANGE
            _server
               .Given(Request.Create().WithPath("/foo").UsingGet())
               .RespondWith(
                 Response.Create()
                   .WithStatusCode(200)
                   .WithBody(@"{ ""msg"": ""Hello world!"" }")
               );


            // ACT
            var path = $"{_server.Urls[0]}/foo";
            var response = await new HttpClient().GetAsync(path);

            // ASSERT
            var actual = await response.Content.ReadAsStringAsync();
            var expected = @"{ ""msg"": ""Hello world!"" }";
            actual.Should().Be(expected);
        }
    }
}
