namespace ExpressionParser.Language.Expressions
{
    public interface IGrammarTerm : ITerm
    {
        void Accept(ITermVisitor visitor);
    }
}