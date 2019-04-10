using System;
using System.Collections.Generic;
using System.Linq;
using ExpressionParser.Language.Expressions.Generated;

namespace ExpressionParser.Language.Expressions
{
    // because we're parsing 'terms' that we'll need to hydrate first with additional information, 
    // we need to have an intermediate step that will give us the opportunity to go and fetch
    // the necessary information (i.e. value of currency pair, or uom conversion factors) and set 
    // the values first, before we get the final result of our "expression"
    public class MyGrammarExpressionVisitor : MyGrammarBaseVisitor<Func<IReadOnlyList<IGrammarTerm>, decimal>>
    {
        private readonly List<IGrammarTerm> _terms = new List<IGrammarTerm>();
        public IReadOnlyList<IGrammarTerm> Terms => _terms.AsReadOnly();
        
        // Math operators
        // ignore any input for these, as they always return a number we can just work with them.
        public override Func<IReadOnlyList<IGrammarTerm>, decimal> VisitNum(MyGrammarParser.NumContext context)
        {
            // ignore any input here, as this is a number we can just return it.
            return _ => decimal.Parse(context.GetText());
        }

        public override Func<IReadOnlyList<IGrammarTerm>, decimal> VisitMulDiv(MyGrammarParser.MulDivContext context)
        {
            var left = Visit(context.expr(0));
            var right = Visit(context.expr(1));

            if (context.op.Type == MyGrammarParser.MUL) return x => left(x) * right(x);
            return _ => left(_) / right(_);
        }

        public override Func<IReadOnlyList<IGrammarTerm>, decimal> VisitAddSub(MyGrammarParser.AddSubContext context)
        {
            var left = Visit(context.expr(0));
            var right = Visit(context.expr(1));

            if (context.op.Type == MyGrammarParser.ADD) return x => left(x) + right(x);
            return _ => left(_) - right(_);
        }

        public override Func<IReadOnlyList<IGrammarTerm>, decimal> VisitUomConvertFunc(MyGrammarParser.UomConvertFuncContext context)
        {
            var fromUom = context.fromUomCode().GetText();
            var toUom = context.toUomCode().GetText();
            var fromToUomConversionCode = $"{fromUom}{toUom}";
            // Todo: Come back to this ID.
            _terms.Add(new UomConvertTerm(fromToUomConversionCode, fromUom, toUom)); 
            return x => x.First(t => t.TermId == fromToUomConversionCode).Value;
        }

        public override Func<IReadOnlyList<IGrammarTerm>, decimal> VisitParens(MyGrammarParser.ParensContext context)
        {
            // if we have a parenthesis, there's an expression inside
            // (that can be any of the allowed operators), so we visit that. 
            return Visit(context.expr());
        }
    }
}