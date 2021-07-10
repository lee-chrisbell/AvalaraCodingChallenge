using System.Threading.Tasks;
using AvalaraCodingChallenge.TaxCalculator.Domain.Tax;

namespace AvalaraCodingChallenge.TaxCalculator.Domain.Tax.Repositories
{
    public interface ITaxRepository
    {
        Task<City> GetCityTaxInformationAsync(string state, string city);
    }
}