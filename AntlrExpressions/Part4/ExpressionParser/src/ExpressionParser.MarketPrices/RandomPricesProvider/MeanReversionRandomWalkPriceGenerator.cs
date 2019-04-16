using System;
using System.Collections.Generic;
using System.Threading;
using ExpressionParser.MarketPrices.Model;

namespace ExpressionParser.MarketPrices.RandomPricesProvider
{
    // This class uses randomization to generate prices.
    // It is a cut down version of: 
    // https://github.com/AdaptiveConsulting/ReactiveTraderCloud/blob/develop/src/server/Adaptive.ReactiveTrader.Server.Pricing/MeanReversionRandomWalkPriceGenerator.cs
    public sealed class MeanReversionRandomWalkPriceGenerator : IPriceGenerator
    {
        private static readonly ThreadLocal<Random> Random =
            new ThreadLocal<Random>(() => new Random());

        private readonly decimal _initial;
        private readonly int _precision;
        private readonly decimal _reversion;
        private readonly decimal _vol;
        public CurrencyPairIdentifier Identifier { get; }

        public MeanReversionRandomWalkPriceGenerator(
            CurrencyPairIdentifier identifier, decimal initial, int precision,
            decimal reversionCoefficient = 0.001m, decimal volatility = 5m)
        {
            _initial = initial;
            _precision = precision;
            _reversion = reversionCoefficient;
            var power = (decimal) Math.Pow(10, precision);
            _vol = volatility * 1m / power;
            Identifier = identifier;
        }

        public IEnumerable<FxRate> Sequence()
        {
            
            var previousMid = _initial;
            while (true)
            {
                var random = (decimal) Random.Value.NextNormal();
                previousMid += _reversion * (_initial - previousMid) + random * _vol;
                var mid = Format(previousMid);
                yield return new FxRate(Identifier, mid);
            }
            // ReSharper disable once IteratorNeverReturns
        }

        private decimal Format(decimal price)
        {
            var power = (decimal) Math.Pow(10, _precision);
            var mid = (int) (price * power);
            return mid / power;
        }
    }
}