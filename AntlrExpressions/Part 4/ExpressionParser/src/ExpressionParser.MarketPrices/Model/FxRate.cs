namespace ExpressionParser.MarketPrices.Model
{
    public class FxRate
    {
        public FxRate(CurrencyPairIdentifier currencyPair, decimal rate)
        {
            CurrencyPair = currencyPair;
            Rate = rate;
        }

        public CurrencyPairIdentifier CurrencyPair { get; }
        public decimal Rate { get; }
    }
}