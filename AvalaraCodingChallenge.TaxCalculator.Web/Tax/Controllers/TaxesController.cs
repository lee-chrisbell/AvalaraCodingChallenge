using System;
using System.Threading.Tasks;
using AvalaraCodingChallenge.TaxCalculator.Domain.Tax.Exceptions;
using AvalaraCodingChallenge.TaxCalculator.Domain.Tax.Services;
using AvalaraCodingChallenge.TaxCalculator.Web.Tax.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AvalaraCodingChallenge.TaxCalculator.Web.Tax.Controllers
{
    /// <summary>
    /// The entrypoint for the application.  It should be fairly logic-free except for 
    ///     parameter validation, error handling, and DTO construction.
    /// </summary>
    [Route("[Controller]")]
    public class TaxesController : Controller
    {
        private readonly ITaxService _service;

        // The logging in this example is lacking, but I do log any uncaught exceptions.
        private readonly ILogger<TaxesController> _logger;
        public TaxesController(ITaxService service, ILogger<TaxesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Calculates the tax amount for the given state, city, and base price.
        /// </summary>
        /// <param name="state">The state for which you want to calculate the tax.</param>
        /// <param name="city">The city for which you want to calculate the tax.</param>
        /// <param name="basePrice">The price of the product without tax</param>
        /// <returns>IActionResult with tax information or an error message.</returns>
        [HttpGet("SalesTax/{state}/{city}")]
        public async Task<IActionResult> GetTaxForCityAsync(string state, string city, decimal basePrice)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(state))
                    return BadRequest(new ErrorDto { ErrorMessage = $"Query parameter {nameof(state)} is required."});

                if(string.IsNullOrWhiteSpace(city))
                    return BadRequest(new ErrorDto { ErrorMessage = $"Query parameter {nameof(city)} is required."});

                if(basePrice <= 0)
                    return BadRequest(new ErrorDto { ErrorMessage = $"Query parameter {nameof(basePrice)} must be greater than 0." });

                var taxAmount = await _service.GetTaxAmountForStateAndCityAsync(state, city, basePrice);

                return Ok(new TaxDto
                {
                    BasePrice = basePrice,
                    TaxAmount = taxAmount
                });
            }
            catch(CityMissingException ex) // Our exception.  OK to return errors to the caller.
            {
                return NotFound(new ErrorDto { ErrorMessage = ex.Message });
            }
            catch(Exception ex) // Don't return a raw error to the caller.
            {
                _logger.LogError(ex, "There was an uncaught exception while calculating the sales tax for this transaction.");
                return StatusCode(500, new ErrorDto { ErrorMessage = "Something went wrong.  Please contact your administrator."});
            }
        }
    }
}