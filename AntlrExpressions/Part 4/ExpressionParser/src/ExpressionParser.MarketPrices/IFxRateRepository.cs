using System;

namespace ExpressionParser.MarketPrices
{
    public interface IFxRateRepository
    {
        IObservable<FxRate> GetPricesFor(CurrencyPairIdentifier currencyPair);
    }
}