using System.Threading.Tasks;

namespace AvalaraCodingChallenge.TaxCalculator.Domain.Tax.Services
{
    public interface ITaxService
    {
        Task<decimal> GetTaxAmountForStateAndCity(string state, string city, decimal basePrice);
    }
}