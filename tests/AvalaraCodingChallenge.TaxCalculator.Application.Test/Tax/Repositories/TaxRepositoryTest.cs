using System.Threading.Tasks;
using AvalaraCodingChallenge.TaxCalculator.Application.Tax.Repositories;
using Moq.AutoMock;
using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using System;
using AvalaraCodingChallenge.TaxCalculator.Infrastructure;
using AvalaraCodingChallenge.TaxCalculator.Domain.Tax;
using AvalaraCodingChallenge.TaxCalculator.Domain.Tax.Exceptions;

namespace AvalaraCodingChallenge.TaxCalculator.Application.Test.Tax.Repositories
{
    public class TaxRepositoryTest
    {
        private AutoMocker _mocker = new AutoMocker(Moq.MockBehavior.Loose);

        private TaxRepository CreateTaxRepository()
        {
            var bld = new DbContextOptionsBuilder();
            bld.UseInMemoryDatabase(Guid.NewGuid().ToString());
            _mocker.Use<DbContext>(new TaxContext(bld.Options));
            return _mocker.CreateInstance<TaxRepository>();
        }

        [Fact]
        public async Task GetCityTaxInformationAsync_ShouldReturnCity_WhenFound()
        {
            var unitUnderTest = CreateTaxRepository();

            var ctx = _mocker.Get<DbContext>();
            var state = new State("NC", 1);
            ctx.Set<State>().Add(state);
            
            state.AddCity("Charlotte", 7.25m / 100m);

            ctx.SaveChanges();

            var res = await unitUnderTest.GetCityTaxInformationAsync("NC", "Charlotte");

            Assert.Equal(.0725m, res.TaxRate);
        }

        [Fact]
        public async Task GetCityTaxInformationAsync_ShouldThrowException_WhenNotFound()
        {
            var unitUnderTest = CreateTaxRepository();

            var ctx = _mocker.Get<DbContext>();
            var state = new State("OH", 1);
            ctx.Set<State>().Add(state);
            
            state.AddCity("Charlotte", 7.25m / 100m);

            ctx.SaveChanges();

            await Assert.ThrowsAsync<CityMissingException>(() => unitUnderTest.GetCityTaxInformationAsync("NC", "Charlotte"));
        }
    }
}