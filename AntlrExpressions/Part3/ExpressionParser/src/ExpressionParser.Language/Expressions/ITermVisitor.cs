using System.Collections.Generic;

namespace ExpressionParser.Language.Expressions
{
    public interface ITermVisitor
    {
        IReadOnlyList<IGrammarTerm> GetAllTerms();
        void Visit(UomConvertTerm uomConversionTerm);
    }
}