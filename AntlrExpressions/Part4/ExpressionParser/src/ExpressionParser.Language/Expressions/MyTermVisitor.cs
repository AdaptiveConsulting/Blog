using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using ExpressionParser.MarketPrices.Repository;
using ExpressionParser.ReferentialData.UoM;

namespace ExpressionParser.Language.Expressions
{
    public class MyTermVisitor : ITermVisitor
    {
        private readonly List<IObservable<IGrammarTerm>> _terms = new List<IObservable<IGrammarTerm>>();
        private readonly IUnitConverter _unitConverter;
        
        // TODO use a container for this type of thing.
        private readonly IFxRateRepository _fxRateRepository;

        public MyTermVisitor(IUnitConverter unitConverter, IFxRateRepository fxRateRepository)
        {
            _unitConverter = unitConverter;
            _fxRateRepository = fxRateRepository;
        }
        
        public IObservable<IList<IGrammarTerm>> GetAllTerms() => _terms.CombineLatest();
        
        public void Visit(UomConvertTerm uomConversionTerm)
        {
            var conversionFactor =
                _unitConverter.GetConversionFactor(uomConversionTerm.FromUom, uomConversionTerm.ToUom);

            uomConversionTerm.SetValue(conversionFactor);

            // It could be that unit converter was a stream as well.
            // but we don't need to go that far.
            // It's fine for it to just return. 
            _terms.Add(Observable.Return(uomConversionTerm));
        }

        public void Visit(FxRateTerm fxRateTerm)
        {
            var priceStream = _fxRateRepository
                .GetPricesFor(fxRateTerm.Identifier)
                .Select(x =>
                {
                    fxRateTerm.SetValue(x.Rate);
                    return fxRateTerm;
                });
            
            _terms.Add(priceStream);
        }
    }
}