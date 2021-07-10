using AvalaraCodingChallenge.TaxCalculator.Domain.Tax;
using Microsoft.EntityFrameworkCore;

namespace AvalaraCodingChallenge.TaxCalculator.Infrastructure
{
    public class TaxContext : DbContext
    {
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        
        public TaxContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}