using System;

namespace ExpressionParser.MarketPrices
{
    public class CurrencyPairIdentifier : IEquatable<CurrencyPairIdentifier>
    {
        public string BaseCurrency { get; }
        public string ContraCurrency { get; }

        public CurrencyPairIdentifier(string baseCurrency,
            string contraCurrency)
        {
            BaseCurrency = baseCurrency;
            ContraCurrency = contraCurrency;
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
            if (obj.GetType() != this.GetType()) return false;
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