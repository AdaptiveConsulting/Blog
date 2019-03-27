namespace ExpressionParser.Language.Expressions
{
    public class UomConvertTerm : ExpressionTermBase, IGrammarTerm
    {
        public UomConvertTerm(string termId, string fromUom, string toUom) : base(termId)
        {
            FromUom = fromUom;
            ToUom = toUom;
        }

        public string FromUom { get; }

        public string ToUom { get; }

        public void Accept(ITermVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}