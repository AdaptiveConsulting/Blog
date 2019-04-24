using ExpressionParser.MarketPrices.Model;

namespace ExpressionParser.Language.Expressions
{
    public class FxRateTerm : ExpressionTermBase, IGrammarTerm
    {
        public CurrencyPairIdentifier Identifier { get; }

        public FxRateTerm(string termId, CurrencyPairIdentifier identifier) :
            base(termId)
        {
            Identifier = identifier;
        }

        public void Accept(ITermVisitor visitor) => visitor.Visit(this);
    }
}