namespace AvalaraCodingChallenge.TaxCalculator.Domain.Tax
{
    public class City
    {
        internal City(decimal taxRate, string name, int id = 0)
        {
            TaxRate = taxRate;
            Id = id;
            Name = name;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public decimal TaxRate { get; private set; }
        public State State { get; private set; }
    }
}