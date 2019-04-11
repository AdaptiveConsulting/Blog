using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using ExpressionParser.MarketPrices.Model;
using ExpressionParser.MarketPrices.Repository;
using NUnit.Framework;

namespace ExpressionParser.MarketPrices.Tests
{
    [TestFixture]
    public class FxRepositoryTests
    {
        private IFxRateRepository _sut;
        private IDisposable _subscription;

        [OneTimeSetUp]
        public void OneTimeSetUp() => _sut = new FxRateRepository();


        [OneTimeTearDown]
        public void OneTimeTearDown() => _subscription?.Dispose();

        [Test]
        [Description("Simple test to proof that we're getting things trough for one single CCY Pair.")]
        public void PricesTickThroughForSpecificCurrencyPair()
        {
            var knownCurrencies = _sut.KnownCurrencies;

            var currencyPairIdentifier = knownCurrencies.First();

            var priceStream = _sut.GetPricesFor(currencyPairIdentifier);

            var fxRates = new List<FxRate>();
            var resetHandle = new AutoResetEvent(false);
            _subscription = priceStream.Buffer(5).Take(1).Subscribe(x =>
            {
                fxRates.AddRange(x);
                if (fxRates.Count >= 5) resetHandle.Set();

            });

            resetHandle.WaitOne(TimeSpan.FromSeconds(5));

            Assert.That(fxRates, Has.Count.EqualTo(5));
            Assert.That(fxRates.All(x => x.CurrencyPair.Equals(currencyPairIdentifier)), Is.True);
        }

        [Test]
        public void PricesTickThroughForAllCurrencies()
        {
            var fxRates = new List<FxRate>();
            var resetHandle = new AutoResetEvent(false);
            const int count = 5;
            _subscription = _sut.GetAllPrices().Take(count).Subscribe(t =>
            {
                fxRates.Add(t);
                if (fxRates.Count >= count) resetHandle.Set();
            });

            resetHandle.WaitOne(TimeSpan.FromSeconds(5));
            Assert.That(fxRates, Has.Count.EqualTo(count));
        }
    }
}