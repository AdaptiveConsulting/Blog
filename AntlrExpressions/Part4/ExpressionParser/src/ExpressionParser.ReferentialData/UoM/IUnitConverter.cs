namespace ExpressionParser.ReferentialData.UoM
{
    public interface IUnitConverter
    {
        decimal GetConversionFactor(string fromCode, string toCode);
    }
}