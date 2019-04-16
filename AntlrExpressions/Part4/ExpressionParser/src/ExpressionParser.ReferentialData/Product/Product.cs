using System;

namespace ExpressionParser.ReferentialData.Product
{
    public class Product : IEquatable<Product>
    {
        public Product(string name, string priceExpression)
        {
            Name = name;
            PriceExpression = priceExpression;
        }

        public string Name { get; }      
        public string PriceExpression { get; }

        public bool Equals(Product other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name) && string.Equals(PriceExpression, other.PriceExpression);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Product) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Name.GetHashCode() * 397) ^ PriceExpression.GetHashCode();
            }
        }

        public override string ToString() => $"'Name: {Name}. Pricing expression: {PriceExpression}";
    }
}