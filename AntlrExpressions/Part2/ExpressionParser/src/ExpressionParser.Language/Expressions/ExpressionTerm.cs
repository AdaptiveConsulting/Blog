namespace ExpressionParser.Language.Expressions
{
    public class ExpressionTerm
    {
        public ExpressionTerm(string termId)
        {
            TermId = termId;
        }

        public string TermId { get; }
        public decimal Value { get; private set; }
        
        public void SetValue(decimal value) => Value = value;
    }
}