using AvalaraCodingChallenge.TaxCalculator.Domain.Tax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AvalaraCodingChallenge.TaxCalculator.Infrastructure.EntityConfigurations
{
    public class CityEntityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasKey(s => s.Id);
            
            builder.Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();

            builder.Property(s => s.TaxRate)
            .HasPrecision(5, 4);
        }
    }
}