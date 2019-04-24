using System.Collections.Generic;
using ExpressionParser.ReferentialData;

namespace ExpressionParser.Language.Expressions
{
    public class MyTermVisitor : ITermVisitor
    {
        private readonly IUnitConverter _unitConverter = new UnitConverter();
        private List<IGrammarTerm> _terms = new List<IGrammarTerm>();
        public IReadOnlyList<IGrammarTerm> GetAllTerms() => _terms.AsReadOnly();
        
        public void Visit(UomConvertTerm uomConversionTerm)
        {
            var conversionFactor =
                _unitConverter.GetConversionFactor(uomConversionTerm.FromUom, uomConversionTerm.ToUom);
            uomConversionTerm.SetValue(conversionFactor);
            _terms.Add(uomConversionTerm);
        }
    }
}