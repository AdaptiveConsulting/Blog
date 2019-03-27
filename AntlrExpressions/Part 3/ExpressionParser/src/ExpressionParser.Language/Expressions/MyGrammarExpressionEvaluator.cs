using System;
using System.Collections.Generic;
using System.IO;
using Antlr4.Runtime;
using ExpressionParser.Language.Expressions.Generated;

namespace ExpressionParser.Language.Expressions
{
    public static class MyGrammarExpressionEvaluator
    {
        public static (Func<IReadOnlyList<IGrammarTerm>,decimal> function, IReadOnlyList<IGrammarTerm> rawTerms) EvaluateExpression(string expression)
        {
            using (var stringReader = new StringReader(expression))
            {
                var inputStream = new AntlrInputStream(stringReader);
                var lexer = new MyGrammarLexer(inputStream);
                var tokens = new CommonTokenStream(lexer);
                var parser = new MyGrammarParser(tokens);
                var expressionTree = parser.expr();// lets parse the whole thing
                var visitor = new MyGrammarExpressionVisitor();
                // now that we have a "parsed expression" let's visit all the nodes.
                var func = visitor.Visit(expressionTree);
                return (func, visitor.Terms);
            }
        }
    }
}