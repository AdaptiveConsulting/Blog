namespace ExpressionParser.Language.Expressions
{
    public interface ITerm
    {
        string TermId { get; }
        decimal Value { get; }
        void SetValue(decimal value);
    }
}