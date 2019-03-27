using System.Collections.Generic;
using System.Linq;
using ExpressionParser.Language.Expressions;
using ExpressionParser.Language.Expressions.Generated;
using NUnit.Framework;

namespace ExpressionParser.Tests
{
    public class ExpressionParsingTests
    {
        [Test]
        public void VisitNumberExpressionWorks()
        {
            const string numberExpression = "2";
            var evaluator = MyGrammarExpressionEvaluator.EvaluateExpression(numberExpression);
            var result = evaluator.function.Invoke(new List<IGrammarTerm>());
            
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
            var evaluator = MyGrammarExpressionEvaluator.EvaluateExpression(numberExpression);
            var result = evaluator.function.Invoke(new List<IGrammarTerm>());
            
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void UomFactorWorks()
        {
            ITermVisitor myTermVisitor = new MyTermVisitor();
            const string inputFunction = "2*UomConvert(MT,Kg)";
            
            var (function, rawTerms) = 
                MyGrammarExpressionEvaluator.EvaluateExpression(inputFunction);
            
            rawTerms.ToList().ForEach(t => t.Accept(myTermVisitor)); // hydrate
            var hydratedTerms = myTermVisitor.GetAllTerms();
            
            var result = function.Invoke(hydratedTerms);
            
            Assert.That(result, Is.EqualTo(2000m));
        }
    }
}
