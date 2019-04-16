using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExpressionParser.Language.Expressions;
using ExpressionParser.MarketPrices.Repository;
using ExpressionParser.ReferentialData.UoM;
using ExpressionParser.Tests.Common;
using NUnit.Framework;

namespace ExpressionParser.Tests
{
    [TestFixture]
    public class ExpressionParsingTests
    {
        [Test]
        public void VisitNumberExpressionWorks()
        {
            const string numberExpression = "2";
            var (function, _) = MyGrammarExpressionEvaluator.EvaluateExpression(numberExpression);
            var result = function.Invoke(new List<IGrammarTerm>());
            
            Assert.That(result, Is.EqualTo(2m));
        }
        
        [TestCase("2+2", 4)]
        [TestCase("2*2", 4)]
        [TestCase("2-2", 0)]
        [TestCase("2/2", 1)]
        [TestCase("(2+2)", 4)]
        [TestCase("(2+2)*2", 8)]
        [TestCase("(2+2)/2", 2)]
        public void BasicMathsWork(string numberExpression, decimal expectedResult)
        {
            var (function, _) = MyGrammarExpressionEvaluator.EvaluateExpression(numberExpression);
            var result = function.Invoke(new List<IGrammarTerm>());
            
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void UomFactorWorks()
        {
            ITermVisitor myTermVisitor = new MyTermVisitor(new UnitConverter(), new FxRateRepository(new TestFxPriceSourceStub()));
            const string inputFunction = "2*UomConvert(MT,Kg)";
            
            var (function, rawTerms) = 
                MyGrammarExpressionEvaluator.EvaluateExpression(inputFunction);
            
            rawTerms.ToList().ForEach(t => t.Accept(myTermVisitor)); // hydrate
            var hydratedTerms = new List<IGrammarTerm>();

            var signal = new AutoResetEvent(false);

            var disposable = myTermVisitor.GetAllTerms().Take(1).Subscribe(terms =>
            {
                hydratedTerms.AddRange(terms);
                signal.Set();
            });

            signal.WaitOne();

            disposable.Dispose();
            
            var result = function.Invoke(hydratedTerms);
            
            Assert.That(result, Is.EqualTo(2000m));
        }

        [Test]
        public async Task FxRateTicksWork()
        {
            // Add test price source.
            ITermVisitor myTermVisitor = new MyTermVisitor(new UnitConverter(), new FxRateRepository(new TestFxPriceSourceStub()));
            const string inputFunction = "FXRate(EURUSD)";
            
            var (function, rawTerms) = 
                MyGrammarExpressionEvaluator.EvaluateExpression(inputFunction);
            
            rawTerms.ToList().ForEach(t => t.Accept(myTermVisitor)); // hydrate

            var hydratedTerms = await myTermVisitor.GetAllTerms().Take(1);

            var result = function.Invoke(hydratedTerms as IReadOnlyList<IGrammarTerm>);

            // the actual rate is not testable as is.
            // we'll come back to this hence the (within)
            Assert.That(result, Is.EqualTo(1m));
        }
    }
}
