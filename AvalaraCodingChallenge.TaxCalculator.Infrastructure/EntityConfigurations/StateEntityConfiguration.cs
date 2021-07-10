using AvalaraCodingChallenge.TaxCalculator.Domain.Tax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AvalaraCodingChallenge.TaxCalculator.Infrastructure.EntityConfigurations
{
    public class StateEntityConfiguration : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder
                .HasKey(s => s.Id);

            builder.Property(s => s.StateCode)
                .HasMaxLength(2);

            builder.HasMany(s => s.Cities)
                .WithOne(c => c.State)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}