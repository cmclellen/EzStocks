using AutoFixture;
using AutoFixture.AutoMoq;
using EzStocks.Api.Infrastructure.PolygonIO;
using Microsoft.Extensions.Options;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace EzStocks.Api.Infrastructure.UnitTests.PolygonIO
{
    [TestClass]
    public class PolygonIOStocksApiClientTests : VerifyBase
    {
        private WireMockServer _server = null!;
        private IFixture _fixture = null!;
        private PolygonIOStocksApiClient _sut = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _server = WireMockServer.Start();

            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            var apiBaseUrl = $"{_server.Urls[0]}";
            var polygonIOSettingsOptions = Options.Create(new PolygonIOSettings { ApiBaseUrl = apiBaseUrl, ApiKey = "a_secret_key" });
            _fixture.Inject(polygonIOSettingsOptions);
            _sut = _fixture.Create<PolygonIOStocksApiClient>();
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
            var responseBody = File.ReadAllText("PolygonIO\\v3_reference_tickers.json");
            _server
               .Given(Request.Create().WithPath("/v3/reference/tickers").UsingGet())
               .RespondWith(
                 Response.Create()
                   .WithStatusCode(200)
                   .WithBody(responseBody)
               );

            // ACT
            var actual = await _sut.GetAllTickersAsync(new Application.Services.GetAllTickersRequest(), default);

            // ASSERT
            await Verify(actual);
        }
    }
}
