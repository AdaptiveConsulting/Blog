using System;
using System.Collections.Generic;
using System.Linq;
using ExpressionParser.MarketPrices.Model;
using ExpressionParser.MarketPrices.RandomPricesProvider;

namespace ExpressionParser.MarketPrices.Repository
{
    public class FxRateRepository : IFxRateRepository
    {
        /// A modified cut-down PriceSource from
        /// https://github.com/AdaptiveConsulting/ReactiveTraderCloud/blob/develop/src/server/Adaptive.ReactiveTrader.Server.Pricing/PriceSource.cs
        /// This class is meant to act as an infinite source of random prices for currency pairs.
        /// It provides an Observable sequence that anyone can subscribe to 
        private readonly IPriceSource _source;

        public IReadOnlyCollection<CurrencyPairIdentifier> KnownCurrencies =>
            _source.KnownCurrencyPairs;

        public FxRateRepository(IPriceSource priceSource)
        {
            _source = priceSource;
        }

        public IObservable<FxRate> GetPricesFor(CurrencyPairIdentifier currencyPair)
        {
            if (!KnownCurrencies.Any(x => x.Equals(currencyPair)))
                throw new ArgumentOutOfRangeException();

            return _source.GetPriceStream(currencyPair.Symbol);
        }

        public IObservable<FxRate> GetAllPrices() => _source.GetAllPricesStream();
    }
}