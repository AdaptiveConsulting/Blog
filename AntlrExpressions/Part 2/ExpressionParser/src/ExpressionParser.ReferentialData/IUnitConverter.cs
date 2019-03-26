using System.Collections.Generic;

namespace ExpressionParser.ReferentialData
{
    public interface IUnitConverter
    {
        decimal GetConversionFactor(string fromCode, string toCode);
    }
}