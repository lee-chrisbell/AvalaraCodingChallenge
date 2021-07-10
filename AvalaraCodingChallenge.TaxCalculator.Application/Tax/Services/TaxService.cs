using System;
using System.Threading.Tasks;
using AvalaraCodingChallenge.TaxCalculator.Domain.Tax.Repositories;
using AvalaraCodingChallenge.TaxCalculator.Domain.Tax.Services;

namespace AvalaraCodingChallenge.TaxCalculator.Application.Tax.Services
{
    /// <summary>
    /// Contains the business logic to calculate taxes.
    /// </summary>
    public class TaxService : ITaxService
    {
        private readonly ITaxRepository _taxRepository;

        public TaxService(ITaxRepository taxRepository)
        {
            _taxRepository = taxRepository;
        }

        /// <summary>
        /// Gets the tax amount for the city, rounded to the nearest 2 decimal points.
        /// </summary>
        /// <param name="state">The state name.</param>
        /// <param name="city">The city name</param>
        /// <param name="basePrice">The price of the item</param>
        /// <returns></returns>
        public async Task<decimal> GetTaxAmountForStateAndCity(string state, string city, decimal basePrice)
        {
            var cityTaxInfo = await _taxRepository.GetCityTaxInformationAsync(state, city);

            return Math.Round((cityTaxInfo.TaxRate * basePrice), 2);
        }
    }
}