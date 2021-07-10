using Xunit;
using Microsoft.Extensions.DependencyInjection;
using AvalaraCodingChallenge.TaxCalculator.Web.Tax.Controllers;
using AvalaraCodingChallenge.TaxCalculator.Domain.Tax.Services;
using Microsoft.Extensions.Logging;

namespace AvalaraCodingChallenge.TaxCalculator.Web.Test
{
    public class StartupTest
    {
        [Fact]
        public void Startup_ShouldRegisterEverythingNeeded_WhenTaxControllerIsBroughtUp()
        {
            var bld = Program.CreateHostBuilder(new string[0]);
            var host = bld.Build();

            var ctrl = new TaxesController(host.Services.GetRequiredService<ITaxService>(), host.Services.GetRequiredService<ILogger<TaxesController>>());

            // The lack of exception is enough of a test here.
        }
    }
}