using System.Collections.Generic;

namespace AvalaraCodingChallenge.TaxCalculator.Domain.Tax
{
    public class State
    {
        private List<City> _cities = new List<City>();

        public State(string stateCode, int id = 0)
        {
            StateCode = stateCode;
            Id = id;
        }

        public void AddCity(string cityName, decimal taxRate, int id = 0)
        {
            _cities.Add(new City(taxRate, cityName, id));
        }

        public int Id { get; private set; }
        public string StateCode { get; private set; }
        public IEnumerable<City> Cities => _cities;
    }
}