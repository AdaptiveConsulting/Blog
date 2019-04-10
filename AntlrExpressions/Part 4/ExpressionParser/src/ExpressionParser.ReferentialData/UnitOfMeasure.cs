using System;

namespace ExpressionParser.ReferentialData
{
    public class UnitOfMeasure
    {
        public UnitOfMeasure(string code, decimal unitsPerMetricTon)
        {
            Code = code;
            UnitsPerMetricTon = unitsPerMetricTon;
        }

        public string Code { get; }
        public decimal UnitsPerMetricTon { get; }
    }
}