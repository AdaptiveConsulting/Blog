using System;

namespace ExpressionParser.MarketPrices.Model
{
    public class CurrencyPairIdentifier : IEquatable<CurrencyPairIdentifier>
    {
        public string BaseCurrency { get; }
        public string ContraCurrency { get; }
        public string Symbol => $"{BaseCurrency}{ContraCurrency}";

        public CurrencyPairIdentifier(string baseCurrency,
            string contraCurrency)
        {
            BaseCurrency = baseCurrency;
            ContraCurrency = contraCurrency;
        }

        public static CurrencyPairIdentifier ParseExact(string currencyPair)
        {
            if (currencyPair.Length != 6)
                throw new ArgumentException("Invalid currency pair.");

            var baseCurrency = currencyPair.Substring(0, 3);
            var contraCurrency = currencyPair.Substring(3, 3);

            return new CurrencyPairIdentifier(baseCurrency, contraCurrency);
        }

        public bool Equals(CurrencyPairIdentifier other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(BaseCurrency, other.BaseCurrency) &&
                   string.Equals(ContraCurrency, other.ContraCurrency);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((CurrencyPairIdentifier) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (BaseCurrency.GetHashCode() * 397) ^
                       ContraCurrency.GetHashCode();
            }
        }
    }
}