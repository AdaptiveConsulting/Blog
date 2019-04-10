using System;
using ExpressionParser.MarketPrices.Model;

namespace ExpressionParser.MarketPrices.Repository
{
    public interface IFxRateRepository
    {
        IObservable<FxRate> GetPricesFor(CurrencyPairIdentifier currencyPair);
    }
}