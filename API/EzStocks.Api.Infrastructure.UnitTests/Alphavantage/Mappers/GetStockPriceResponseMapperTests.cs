using EzStocks.Api.Infrastructure.Alphavantage.Mappers;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EzStocks.Api.Infrastructure.UnitTests.Alphavantage.Mappers
{
    [TestClass]
    public class GetStockPriceResponseMapperTests
    {

        private IFixture _fixture = null!;
        private GetStockPriceResponseMapper _sut = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _sut = _fixture.Create<GetStockPriceResponseMapper>();
        }

        [TestMethod]
        public void MapFromText_ShouldReturnResponse()
        {
            // ARRANGE

            // ACT
            

            // ASSERT
        }
    }
}
