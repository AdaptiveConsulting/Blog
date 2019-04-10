using System;
using System.Collections.Generic;
using System.Linq;
using ExpressionParser.Language.Expressions.Generated;

namespace ExpressionParser.Language.Expressions
{
    public class MyGrammarExpressionVisitor : MyGrammarBaseVisitor<Func<IReadOnlyList<ExpressionTerm>, decimal>>
    {
        private readonly List<ExpressionTerm> _terms = new List<ExpressionTerm>();
        public IReadOnlyList<ExpressionTerm> Terms => _terms.AsReadOnly();
        
        public override Func<IReadOnlyList<ExpressionTerm>, decimal> VisitNum(MyGrammarParser.NumContext context)
        {
            return _ => decimal.Parse(context.GetText());
        }

        public override Func<IReadOnlyList<ExpressionTerm>, decimal> VisitMulDiv(MyGrammarParser.MulDivContext context)
        {
            var left = Visit(context.expr(0));
            var right = Visit(context.expr(1));

            if (context.op.Type == MyGrammarParser.MUL) return x => left(x) * right(x);
            return x => left(x) / right(x);
        }

        public override Func<IReadOnlyList<ExpressionTerm>, decimal> VisitAddSub(MyGrammarParser.AddSubContext context)
        {
            var left = Visit(context.expr(0));
            var right = Visit(context.expr(1));

            if (context.op.Type == MyGrammarParser.ADD) return x => left(x) + right(x);
            return x => left(x) - right(x);
        }

        public override Func<IReadOnlyList<ExpressionTerm>, decimal> VisitFxRateFunc(MyGrammarParser.FxRateFuncContext context)
        {
            var currencyPair = context.currencyPair().GetText();
            _terms.Add(new ExpressionTerm(currencyPair));
            return x => x.First(y => y.TermId == currencyPair).Value;
        }

        public override Func<IReadOnlyList<ExpressionTerm>, decimal> VisitUomConvertFunc(MyGrammarParser.UomConvertFuncContext context)
        {
            var fromUom = context.fromUomCode().GetText();
            var toUom = context.toUomCode().GetText();
            var fromToUomConversionCode = $"{fromUom}{toUom}";
            _terms.Add(new ExpressionTerm(fromToUomConversionCode)); // Todo: think of a better way.
            return x => x.First(t => t.TermId == fromToUomConversionCode).Value;
        }

        public override Func<IReadOnlyList<ExpressionTerm>, decimal> VisitParens(MyGrammarParser.ParensContext context)
        {
            // if we have a parenthesis, there's an expression inside (that can be any of the allowed operators), so we visit that. 
            return Visit(context.expr());
        }
    }
}