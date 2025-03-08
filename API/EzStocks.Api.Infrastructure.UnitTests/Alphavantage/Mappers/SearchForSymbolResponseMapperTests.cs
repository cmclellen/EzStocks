using AutoFixture;
using AutoFixture.AutoMoq;
using EzStocks.Api.Infrastructure.Alphavantage.Mappers;
using EzStocks.Api.Infrastructure.UnitTests.Helpers;

namespace EzStocks.Api.Infrastructure.UnitTests.Alphavantage.Mappers
{
    [TestClass]
    public class SearchForSymbolResponseMapperTests : VerifyMSTest.VerifyBase
    {
        private IFixture _fixture = null!;
        private SearchForSymbolResponseMapper _sut = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _sut = _fixture.Create<SearchForSymbolResponseMapper>();
        }

        [TestMethod]
        public async Task MapFromJson_ShouldReturnSearchForSymbolResponse()
        {
            // ARRANGE
            var json = await FileHelpers.GetFileContentAsync(Path.Join("Mappers", "SYMBOL_SEARCH.Response.json"), CancellationToken.None);

            // ACT
            var actual = _sut.MapFromJson(json);

            // ASSERT
            await Verify(actual).DontScrubDateTimes();
        }
    }
}
