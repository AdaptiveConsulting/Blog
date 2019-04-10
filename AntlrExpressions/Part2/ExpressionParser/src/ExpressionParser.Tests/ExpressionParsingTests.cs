using System.Collections.Generic;
using ExpressionParser.Language.Expressions;
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
            var result = evaluator.Invoke(new List<ExpressionTerm>());
            
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
            var result = evaluator.Invoke(new List<ExpressionTerm>());
            
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
