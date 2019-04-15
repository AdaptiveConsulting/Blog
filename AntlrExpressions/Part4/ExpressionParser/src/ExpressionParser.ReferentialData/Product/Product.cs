namespace ExpressionParser.ReferentialData.Product
{
    public class Product
    {
        public Product(string name, string priceExpression)
        {
            Name = name;
            PriceExpression = priceExpression;
        }

        public string Name { get; }      
        public string PriceExpression { get; }
    }
}