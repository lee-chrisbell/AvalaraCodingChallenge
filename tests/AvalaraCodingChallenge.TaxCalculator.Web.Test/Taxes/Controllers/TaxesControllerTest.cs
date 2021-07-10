using System.Threading.Tasks;
using AvalaraCodingChallenge.TaxCalculator.Domain.Tax.Exceptions;
using AvalaraCodingChallenge.TaxCalculator.Domain.Tax.Services;
using AvalaraCodingChallenge.TaxCalculator.Web.Tax.Controllers;
using AvalaraCodingChallenge.TaxCalculator.Web.Tax.Dtos;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace AvalaraCodingChallenge.TaxCalculator.Web.Test.Taxes.Controllers
{
    public class TaxesControllerTest
    {
        private AutoMocker _mocker = new AutoMocker(Moq.MockBehavior.Loose);

        private TaxesController CreateTaxesController()
        {
            return _mocker.CreateInstance<TaxesController>();
        }

        [Fact]
        public async Task GetTaxForCity_ShouldReturnDto_WhenValuesValid()
        {
            var unitUnderTest = CreateTaxesController();

            _mocker.GetMock<ITaxService>()
                .Setup(s => s.GetTaxAmountForStateAndCity("OH", "Miami", 12.40m))
                .ReturnsAsync(.90m);

            var result = await unitUnderTest.GetTaxForCity("OH", "Miami", 12.40m) as ObjectResult;
            var taxInfo = result.Value as TaxDto;
            Assert.Equal(12.40m, taxInfo.BasePrice);
            Assert.Equal(.90m, taxInfo.TaxAmount);
            Assert.Equal(13.30m, taxInfo.PriceAfterTax);
        }

        [Fact]
        public async Task GetTaxForCity_ShouldReturnBadRequest_WhenStateIsInvalid()
        {
            var unitUnderTest = CreateTaxesController();

            var result = await unitUnderTest.GetTaxForCity("", "Miami", 12.40m) as ObjectResult;

            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Query parameter state is required.", (result.Value as ErrorDto).ErrorMessage);
        }

        [Fact]
        public async Task GetTaxForCity_ShouldReturnBadRequest_WhenCityIsInvalid()
        {
            var unitUnderTest = CreateTaxesController();

            var result = await unitUnderTest.GetTaxForCity("OH", "", 12.40m) as ObjectResult;

            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Query parameter city is required.", (result.Value as ErrorDto).ErrorMessage);
        }

        [Fact]
        public async Task GetTaxForCity_ShouldReturnBadRequest_WhenBasePriceIsInvalid()
        {
            var unitUnderTest = CreateTaxesController();

            var result = await unitUnderTest.GetTaxForCity("OH", "Miami", -12.40m) as ObjectResult;

            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Query parameter basePrice must be greater than 0.", (result.Value as ErrorDto).ErrorMessage);
        }

        [Fact]
        public async Task GetTaxForCity_ShouldReturn500_WhenExceptionIsThrown()
        {
            var unitUnderTest = CreateTaxesController();

            _mocker.GetMock<ITaxService>()
                .Setup(s => s.GetTaxAmountForStateAndCity("OH", "Miami", 12.40m))
                .Throws(new System.Exception("Broken DB Connection or something"));

            var result = await unitUnderTest.GetTaxForCity("OH", "Miami", 12.40m) as ObjectResult;

            Assert.Equal(500, result.StatusCode);
            Assert.Equal("Something went wrong.  Please contact your administrator.", (result.Value as ErrorDto).ErrorMessage);
        }

        [Fact]
        public async Task GetTaxForCity_ShouldReturn404_WhenCityNotFound()
        {
            var unitUnderTest = CreateTaxesController();

            _mocker.GetMock<ITaxService>()
                .Setup(s => s.GetTaxAmountForStateAndCity("OH", "Miami", 12.40m))
                .Throws(new CityMissingException());

            var result = await unitUnderTest.GetTaxForCity("OH", "Miami", 12.40m) as ObjectResult;

            Assert.Equal(404, result.StatusCode);
            Assert.Equal("We do not have city information for the given city in the given state.", (result.Value as ErrorDto).ErrorMessage);
        }
    }
}