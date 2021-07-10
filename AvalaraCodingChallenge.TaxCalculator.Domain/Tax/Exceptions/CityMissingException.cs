using System;

namespace AvalaraCodingChallenge.TaxCalculator.Domain.Tax.Exceptions
{
    public class CityMissingException : Exception
    {
        public CityMissingException() : base(message: "We do not have city information for the given city in the given state.")
        {

        }
    }
}