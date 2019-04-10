using System.Collections.Generic;
using ExpressionParser.MarketPrices.Model;

namespace ExpressionParser.Language.Expressions
{
    public interface ITermVisitor
    {
        IReadOnlyList<IGrammarTerm> GetAllTerms();
        void Visit(UomConvertTerm uomConversionTerm);
        void Visit(FxRateTerm uomConversionTerm);
    }
}