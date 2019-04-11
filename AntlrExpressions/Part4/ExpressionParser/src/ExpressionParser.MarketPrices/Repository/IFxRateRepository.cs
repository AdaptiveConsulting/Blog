using System;
using System.Collections.Generic;
using ExpressionParser.MarketPrices.Model;

namespace ExpressionParser.MarketPrices.Repository
{
    public interface IFxRateRepository
    {
        IReadOnlyCollection<CurrencyPairIdentifier> KnownCurrencies { get; }
        IObservable<FxRate> GetPricesFor(CurrencyPairIdentifier currencyPair);
        IObservable<FxRate> GetAllPrices();
    }
}