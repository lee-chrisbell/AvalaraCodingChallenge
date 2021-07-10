namespace AvalaraCodingChallenge.TaxCalculator.Web.Tax.Dtos
{
    public class TaxDto
    {
        public decimal BasePrice { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal PriceAfterTax => BasePrice + TaxAmount;
    }
}