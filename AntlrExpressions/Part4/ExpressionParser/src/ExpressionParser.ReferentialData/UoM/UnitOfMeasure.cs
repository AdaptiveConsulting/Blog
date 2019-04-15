namespace ExpressionParser.ReferentialData.UoM
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