using System;
using System.Collections.Generic;

namespace ExpressionParser.Language.Expressions
{
    public interface ITermVisitor
    {
        IObservable<IList<IGrammarTerm>> GetAllTerms();
        void Visit(UomConvertTerm uomConversionTerm);
        void Visit(FxRateTerm uomConversionTerm);
    }
}