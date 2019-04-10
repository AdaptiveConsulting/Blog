namespace ExpressionParser.Language.Expressions
{
    public abstract class ExpressionTermBase : ITerm
    {
        protected ExpressionTermBase(string termId)
        {
            TermId = termId;
        }

        public string TermId { get; }
        public decimal Value { get; private set; }
        
        public void SetValue(decimal value) => Value = value;
    }
}