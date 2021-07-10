using Moq.AutoMock;
using Moq;
using Xunit;
using System.Threading.Tasks;
using AvalaraCodingChallenge.TaxCalculator.Application.Tax.Services;
using AvalaraCodingChallenge.TaxCalculator.Domain.Tax.Repositories;
using AvalaraCodingChallenge.TaxCalculator.Domain.Tax;
using System.Linq;
using AvalaraCodingChallenge.TaxCalculator.Domain.Tax.Exceptions;

namespace AvalaraCodingChallenge.TaxCalculator.Application.Test.Tax.Services
{
    public class TaxServiceTest
    {
        private AutoMocker _mocker = new AutoMocker(Moq.MockBehavior.Loose);

        private TaxService CreateTaxService()
        {
            return _mocker.CreateInstance<TaxService>();
        }

        [Fact]
        public async Task GetTaxAmountForStateAndCity_ShouldGetCityTaxInfo_WhenFoundInRepo()
        {
            var state = new State("OH");
            state.AddCity("Troy", .0725m);

            _mocker.GetMock<ITaxRepository>()
                .Setup(s => s.GetCityTaxInformationAsync("OH", "Troy"))
                .ReturnsAsync(state.Cities.First());
            
            var unitUnderTest = CreateTaxService();

            var res = await unitUnderTest.GetTaxAmountForStateAndCity("OH", "Troy", 12.50m);

            Assert.Equal(.91m, res);
        }

        [Fact]
        public async Task GetTaxAmountForStateAndCity_ShouldBubbleException_WhenNotFoundInRepo()
        {
            _mocker.GetMock<ITaxRepository>()
                .Setup(s => s.GetCityTaxInformationAsync("OH", "Troy"))
                .ThrowsAsync(new CityMissingException());
            
            var unitUnderTest = CreateTaxService();

            await Assert.ThrowsAsync<CityMissingException>(() => unitUnderTest.GetTaxAmountForStateAndCity("OH", "Troy", 12.50m));
        }
    }
}