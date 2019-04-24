using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using ExpressionParser.MarketPrices.Model;
using ExpressionParser.MarketPrices.RandomPricesProvider;

namespace ExpressionParser.Tests.Common
{
    public class TestFxPriceSourceStub : IPriceSource
    {
        public IReadOnlyList<CurrencyPairIdentifier> KnownCurrencyPairs { get; }
            = new[] {CurrencyPairIdentifier.ParseExact(Symbol),  };

        private const string Symbol = "EURUSD";
        
        private readonly FxRate _testRate = new FxRate(CurrencyPairIdentifier.ParseExact(Symbol), decimal.One);
        
        public IObservable<FxRate> GetPriceStream(string symbol)
        {
            return Observable.Return(_testRate);
        }

        public IObservable<FxRate> GetAllPricesStream()
        {
            return Observable.Return(_testRate);
        }
    }
}