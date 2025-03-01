using AutoFixture;
using AutoFixture.AutoMoq;
using EzStocks.Api.Infrastructure.Alphavantage.Mappers;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using System.Reflection;

namespace EzStocks.Api.Infrastructure.UnitTests.Alphavantage.Mappers
{
    [TestClass]
    public class GetStockPriceResponseMapperTests : VerifyMSTest.VerifyBase
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
        public async Task MapFromJson_ShouldReturnGetStockPriceResponse()
        {
            // ARRANGE
            var assembly = Assembly.GetExecutingAssembly();            
            var path = Path.Join(Path.GetDirectoryName(assembly.GetAssemblyLocation()), "Alphavantage", "Mappers");
            var json = await File.ReadAllTextAsync(Path.Join(path, "TIME_SERIES_DAILY.Response.json"));
            
            // ACT
            var actual = _sut.MapFromJson(json);

            // ASSERT
            await Verify(actual).DontScrubDateTimes();
        }
    }
}
