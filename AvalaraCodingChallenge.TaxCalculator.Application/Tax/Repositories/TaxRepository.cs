
using System.Linq;
using System.Threading.Tasks;
using AvalaraCodingChallenge.TaxCalculator.Domain.Tax;
using AvalaraCodingChallenge.TaxCalculator.Domain.Tax.Exceptions;
using AvalaraCodingChallenge.TaxCalculator.Domain.Tax.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AvalaraCodingChallenge.TaxCalculator.Application.Tax.Repositories
{
    /// <summary>
    /// Performs queries and commands against the database.
    /// 
    /// NOTE: I would have a unit of work, too, when we do a full CRUD service.
    /// </summary>
    public class TaxRepository : ITaxRepository
    {
        private readonly DbContext _context;

        public TaxRepository(DbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns the city record that matches the city name and the state name.  Case insensitive.
        /// </summary>
        /// <param name="state">The 2 digit state code</param>
        /// <param name="city">The city name</param>
        /// <returns></returns>
        public async Task<City> GetCityTaxInformationAsync(string state, string city)
        {
            // This uses to lower because .Equals(state, StringComparison.InvariantCultureIgnoreCase) doesn't translate to sql.  
            //      Doesn't matter in this example because it works in in-memory, but it would have failed in a real world scenario.
            var cityTaxInfo = await _context.Set<City>()
                .FirstOrDefaultAsync(c => 
                    c.State.StateCode.ToLower() == state.ToLower() 
                    && c.Name.ToLower() == city.ToLower()
                );

            if(cityTaxInfo == null)
                throw new CityMissingException();

            return cityTaxInfo;
        }
    }
}