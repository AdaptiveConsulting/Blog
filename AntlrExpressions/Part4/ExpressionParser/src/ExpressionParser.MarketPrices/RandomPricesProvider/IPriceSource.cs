using System;
using System.Collections.Generic;
using ExpressionParser.MarketPrices.Model;

namespace ExpressionParser.MarketPrices.RandomPricesProvider
{
    public interface IPriceSource
    {
        IReadOnlyList<CurrencyPairIdentifier> KnownCurrencyPairs { get; }
        IObservable<FxRate> GetPriceStream(string symbol);
        IObservable<FxRate> GetAllPricesStream();
    }
}