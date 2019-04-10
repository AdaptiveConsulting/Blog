using System.Collections.Generic;
using ExpressionParser.MarketPrices.Model;

namespace ExpressionParser.MarketPrices.RandomPricesProvider
{
    public interface IPriceGenerator
    {
        CurrencyPairIdentifier Identifier{ get; }
        IEnumerable<FxRate> Sequence();
    }
}