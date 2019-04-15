using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ExpressionParser.MarketPrices.Model;

namespace ExpressionParser.MarketPrices.RandomPricesProvider
{
    /// <summary>
    /// A modified cut-down PriceSource class from
    /// https://github.com/AdaptiveConsulting/ReactiveTraderCloud/blob/develop/src/server/Adaptive.ReactiveTrader.Server.Pricing/PriceSource.cs
    /// This class is meant to act as an infinite source of random prices for currency pairs.
    /// It provides an Observable sequence that anyone can subscribe to 
    /// </summary>
    public sealed class RandomPriceSource : IDisposable, IPriceSource
    {
        private static readonly Random Random = new Random();
        private const int Precision = 5;

        private readonly Dictionary<string, IObservable<FxRate>> _priceStreams =
            new Dictionary<string, IObservable<FxRate>>();

        private readonly List<IPriceGenerator> _priceGenerators;

        private readonly CompositeDisposable _disposable =
            new CompositeDisposable();

        private readonly IConnectableObservable<long> _timer;

        public IReadOnlyList<CurrencyPairIdentifier> KnownCurrencyPairs =>
            _priceGenerators.Select(x => x.Identifier).Distinct().ToList();

        public RandomPriceSource()
        {
            _timer = Observable.Interval(TimeSpan.FromMilliseconds(50)).Publish();

            _priceGenerators = GetPriceGenerators();

            foreach (var ccy in _priceGenerators)
            {
                var observable = Observable.Create<FxRate>(observer =>
                    {
                        var prices = ccy.Sequence().GetEnumerator();

                        prices.MoveNext();
                        observer.OnNext(prices.Current);

                        var disposable = RegisterPriceTrigger(ccy.Identifier)
                            .Subscribe(o =>
                            {
                                prices.MoveNext();
                                observer.OnNext(prices.Current);
                            });

                        _disposable.Add(disposable);
                        _disposable.Add(prices);

                        return disposable;
                    })
                    .Replay(1)
                    .RefCount();

                _priceStreams.Add(ccy.Identifier.Symbol, observable);
            }

            _timer.Connect();
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }

        public IObservable<FxRate> GetPriceStream(string symbol)
        {
            return _priceStreams[symbol];
        }

        public IObservable<FxRate> GetAllPricesStream()
        {
            return _priceStreams.Values.Merge();
        }

        private IObservable<Unit> RegisterPriceTrigger(CurrencyPairIdentifier identifier)
        {
            return _timer
                .Where(_ =>
                    _priceGenerators[Random.Next(_priceGenerators.Count)].Identifier.Symbol ==
                    identifier.Symbol)
                .Select(_ => Unit.Default);
        }

        private static IPriceGenerator CreatePriceGenerator(CurrencyPairIdentifier symbol,
            decimal initial, int precision)
        {
            return new MeanReversionRandomWalkPriceGenerator(symbol, initial,
                precision);
        }

        private static List<IPriceGenerator> GetPriceGenerators()
        {
            return new List<IPriceGenerator>
            {
                CreatePriceGenerator(
                    new CurrencyPairIdentifier("EUR", "USD"),
                    1.09443m,
                    Precision),
                CreatePriceGenerator(
                    new CurrencyPairIdentifier("USD", "JPY"),
                    121.656m,
                    Precision),
                CreatePriceGenerator(
                    new CurrencyPairIdentifier("GBP", "USD"),
                    1.51746m,
                    Precision),
                CreatePriceGenerator(
                    new CurrencyPairIdentifier("GBP", "JPY"),
                    184.608m,
                    Precision),
                CreatePriceGenerator(
                    new CurrencyPairIdentifier("EUR", "GBP"),
                    0.72123m,
                    Precision),
                CreatePriceGenerator(
                    new CurrencyPairIdentifier("USD", "CHF"),
                    0.98962m,
                    Precision),
                CreatePriceGenerator(
                    new CurrencyPairIdentifier("EUR", "JPY"),
                    133.144m,
                    Precision),
                CreatePriceGenerator(
                    new CurrencyPairIdentifier("EUR", "CHF"),
                    1.08318m,
                    Precision),
                CreatePriceGenerator(
                    new CurrencyPairIdentifier("AUD", "USD"),
                    0.72881m,
                    Precision),
                CreatePriceGenerator(
                    new CurrencyPairIdentifier("NZD", "USD"),
                    0.6729m,
                    Precision)
            };
        }
    }
}